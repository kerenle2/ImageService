using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using ImageService.Modal;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static Client client = null;
        static ConfigModel configModel = new ConfigModel();
        static ImageWebModel imageWebModel = new ImageWebModel();
        static ThumbnailsModel thumbsModel;
        static bool waitForRemoveHandler = false;
        static List<LogModel> logs = new List<LogModel>();
        
        /// <summary>
        /// coonstructor
        /// </summary>
        static FirstController()
        {
            client = Client.getInstance();

            client.DataRecieved += OnDataRecieved;

        }
        // GET: First/ImageWeb
        [HttpGet]
        public ActionResult ImageWeb()
        {
            if (client.Conected)
            {
                imageWebModel.IsConnect = "Server Is Connected";
            }
            else
            {
                imageWebModel.IsConnect = "Server Is Not Connected";
            }
            //divide by 2 for not include the thumbnails
            imageWebModel.ImagesNum = getImagesNum(configModel.outputDir) / 2;
            return View(imageWebModel);
        }

        
        // GET: First/RemoveHandler
        public ActionResult RemoveHandler(string dir)
        {
            configModel.dirToRemove = dir;
            ViewBag.dirToRemove = dir;
            return View();
        }

        // GET: First/Photos
        public ActionResult Photos()
        {
            thumbsModel = new ThumbnailsModel(configModel.outputDir);
            return View(thumbsModel);
        }


        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }
        //get First/Logs
        [HttpGet]
        public ActionResult Logs()
        {
            return View(logs);
        }

        ////post logs!!
        //[HttpPost]
        //public ActionResult Logs(LogModel log_m)
        //{
        //    return View(logs);
        //}

        //get configurations
        [HttpGet]
        public ActionResult Configuration()
        {
            while (waitForRemoveHandler) { }
            ViewBag.outputDir = configModel.outputDir;
            ViewBag.logName = configModel.logName;
            ViewBag.sourceName = configModel.sourceName;
            ViewBag.thumbSize = configModel.thumbSize;
            ViewBag.dirs = configModel.dirs;
            ViewBag.dirToRemove = configModel.dirToRemove;
            return View();
        }

        //post configuration
        [HttpPost]
        public ActionResult Configuration(ConfigModel conf)
        {
            return View(conf);
        }
        
        public ActionResult UpdateDirToRemove(string dir)
        {   if(dir!=null)
            {
                configModel.dirToRemove = dir;
                ViewBag.dirToRemove = dir;
            }
            try
            {
                return View(configModel);

            }
            catch
            {
                return View(configModel);

            }
        }
        /// <summary>
        /// remove a handler
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public ActionResult Remove(string dir)
        {
            if (dir!=null)
            {
                waitForRemoveHandler = true;
                string[] args = { dir };
                client.sendCommandRequest((int)CommandEnum.CloseCommand, args);
                //wait until the dir will be removed
                while (configModel.dirs.Contains(dir)) { }
                waitForRemoveHandler = false;
            }
            return RedirectToAction("Configuration");
        }

        /// <summary>
        /// get the number of images in the output dir
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int getImagesNum(string path)
        {
            try
            {
                //NEED- add all kind og images!!!!!!!!
                var directoryFiles = Directory.EnumerateFiles(path, "*.jpg", SearchOption.AllDirectories);
                //initialize counter
                int counter = 0;
                //loop on file paths

                foreach (string filePath in directoryFiles)
                {
                    counter++;
                }

                return counter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;

            }

        }
        /// <summary>
        /// get the data from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ee"></param>
        public static void OnDataRecieved(object sender, EventArgs ee)
        {
            MsgInfoEventArgs e = (MsgInfoEventArgs)ee;
            if (e.id == MessagesToClientEnum.Logs)
            {
                Console.WriteLine("I know i got an Logs msg!");
                List<Log> logsList = JsonConvert.DeserializeObject<List<Log>>(e.msg);
                //for each log in the list, add it to view.
                foreach (Log log in logsList)
                {
                    logs.Add(new LogModel { Type = log.Type.ToString(), Message = log.Message });
                }
            }
            if (e.id == MessagesToClientEnum.Settings)
            {
                Console.WriteLine("I know i got an settings msg!");
               configModel.AddSettingsFromJson(e.msg);
            }

            if (e.id == MessagesToClientEnum.HandlerRemoved)
            {
                configModel.dirs.Remove(e.msg);
                waitForRemoveHandler = false;
            }
        }

    }
}
