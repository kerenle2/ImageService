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
using System.Web.Hosting;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static Client client = null;
        static ConfigModel configModel = new ConfigModel();
        static ImageWebModel imageWebModel = new ImageWebModel();
        static List<LogModel> logs = new List<LogModel>();
        static ThumbnailsModel thumbsModel;
        static bool waitForRemoveHandler = false;
       
        
        /// <summary>
        /// coonstructor
        /// </summary>
        static FirstController()
        {
            client = Client.getInstance();

            client.DataRecieved += OnDataRecieved;

        }


        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult ViewPhoto( int picNumber = -1)
        {
           // thumbsModel.picToDelete = picNumber;
            foreach (Thumbnail thumb in thumbsModel.thumbs)
            {
                if (thumb.picNumber == picNumber)
                {
                    string pathToPic = GetOriginPhotoFullPathFromThumb(thumb);
                    if (System.IO.File.Exists(pathToPic))
                    {
                        return View(thumb);//chngeeeeeeeeeeeeee
                    }

                }
            }
            return View(); //error??
        }

        public ActionResult DeletePhotoPressed(int picNumber = -1)
        {
         

            thumbsModel.picToDelete = picNumber;
            foreach(Thumbnail thumb in thumbsModel.thumbs)
            {
                if (thumb.picNumber == picNumber)
                {
                    return View(thumb);
                }
            }

            return View(); //error??
        }



        public ActionResult DeletePhoto(int picNumber = -1)
        {

            Thumbnail thumbToDelete = null;

            foreach (Thumbnail thumb in thumbsModel.thumbs)
            {
                if (thumb.picNumber == picNumber)
                {
                    thumbToDelete = thumb;
                    break;
                }
            }
            if(thumbToDelete == null)
            {
                return View(thumbsModel); //return error here
            }
        
            try
            {
                //deldete photo from folder
                string pathToPic = GetOriginPhotoFullPathFromThumb(thumbToDelete);
                //string string1 = thumbToDelete.fullPath;
                //string string2 = "Thumbnails\\";
                //string pathToPic = string1.Replace(string2, "");

                if (System.IO.File.Exists(pathToPic))
                {
                    System.IO.File.Delete(pathToPic);
                }

                //deldete thumb from folder:
                if (System.IO.File.Exists(thumbToDelete.fullPath))
                {
                    System.IO.File.Delete(thumbToDelete.fullPath);
                }
             
            }
            catch(Exception e)
            {
                return RedirectToAction("Photos"); //change here to error or add error msg and then back to photos.
            }
   
            thumbsModel.deleteThumb(thumbToDelete);

            return RedirectToAction("Photos");

        }

        private string GetOriginPhotoFullPathFromThumb(Thumbnail thumb)
        {
            string string1 = thumb.fullPath;
            string string2 = "Thumbnails\\";
            string pathToPic = string1.Replace(string2, "");
            return pathToPic;
        }


        [HttpGet]
        public ActionResult AjaxView()
        {
            return View();
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }

        
        [HttpPost]
        public JObject GetFilteredLog(string type, string msg)
        {
            JObject data = new JObject();
            data["Type"] = type;
            data["Message"] = msg;
            return data;
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

            imageWebModel.ImagesNum = getImagesNum(configModel.outputDir);
            return View(imageWebModel);
        }

     
            public int getImagesNum(string outputDir)
        {
            int count = 0;
            string[] extensions = { ".jpg", ".png", ".gif", ".bmp" };

            if (Directory.Exists(outputDir + "\\Thumbnails"))
            {
                string[] paths = Directory.GetFiles(outputDir + "\\Thumbnails", "*.*", SearchOption.AllDirectories);
                foreach (string path in paths)
                {
                    if (extensions.Contains(Path.GetExtension(path)))
                    {
                        count++;
                    }
                }
            }
            return count;
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

          //  thumbs.Clear(); //yes? im going this way?

            //string root = @HostingEnvironment.MapPath("~/OutputFromApp");
            //var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
            //string relativeThumb = "";
            //foreach (string fullThumb in files)
            //{
            //    relativeThumb = fullThumb.Replace(root, "");
            //    relativeThumb = relativeThumb.TrimStart('\\');

            //}
            //ViewBag.path = relativeThumb;

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
//                return View("ERROR"); MAYBE THAT????????


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
