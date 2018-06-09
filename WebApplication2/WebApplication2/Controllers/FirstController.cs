using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static Client client = null;
        //LogModel logModel = new LogModel();
        static List<Employee> employees = new List<Employee>()
        {
          new Employee  { FirstName = "Moshe", LastName = "Aron", Email = "Stam@stam", Salary = 10000, Phone = "08-8888888" },
          new Employee  { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 2000, Phone = "08-8888888" },
          new Employee   { FirstName = "Mor", LastName = "Sinai", Email = "Stam@stam", Salary = 500, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 20, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 700, Phone = "08-8888888" }
        };
        static List<LogModel> logs = new List<LogModel>()
        {
            new LogModel { Type = "Info" , Message = "testtttt"},
            new LogModel { Type = "Info" , Message = "hey hey"}
        };

        static ConfigModel configModel = new ConfigModel();
      //  static List<string> fields = new List<string>();
        static FirstController()
        {
            client = Client.getInstance();

            client.DataRecieved += OnDataRecieved;

        }
        // GET: First
        public ActionResult Index()
        {
            return View();
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
        public JObject GetEmployee(string name, int salary)
        {
            foreach (var empl in employees)
            {
                if (empl.Salary > salary || name.Equals(name))
                {
                    JObject data = new JObject();
                    data["FirstName"] = empl.FirstName;
                    data["LastName"] = empl.LastName;
                    data["Salary"] = empl.Salary;
                    return data;
                }
            }
            return null;
        }

    

        // GET: First/Details
        public ActionResult Details()
        {
            return View(employees);
        }

        // GET: First/RemoveHandler
        public ActionResult RemoveHandler()
        {
            return View();
        }

        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }
        //get logs!!!!!
        [HttpGet]
        public ActionResult Logs()
        {
            return View(logs);
        }
        //post logs!!
        [HttpPost]
        public ActionResult Logs(LogModel log_m)
        {
            return View(logs);
        }

        //get configurations!! -- ??
        [HttpGet]
        public ActionResult Configuration()
        {
            ViewBag.outputDir = configModel.outputDir;
            ViewBag.logName = configModel.logName;
            ViewBag.sourceName = configModel.sourceName;
            ViewBag.thumbSize = configModel.thumbSize;
            ViewBag.dirs = configModel.dirs;
            return View();
        }

        //post configuration!! --???
        [HttpPost]
        public ActionResult Configuration(ConfigModel conf)
        {
            return View(conf);
        }



        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                employees.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Employee emp in employees) {
                if (emp.ID.Equals(id)) { 
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in employees)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    employees.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }

        public ActionResult UpdateDirToRemove(string dir)
        {
            configModel.dirToRemove = dir;
            ViewBag.dirToRemove = dir;
            return View();
        }

        [HttpGet]
        public ActionResult Remove(string dir)
        {
            string[] args = { dir };
            client.sendCommandRequest((int)CommandEnum.CloseCommand, args);
            
            return RedirectToAction("Configuration");

        }
        //public void AddLog(Log log)
        //{

        //}
        public static void OnDataRecieved(object sender, EventArgs ee)
        {
            MsgInfoEventArgs e = (MsgInfoEventArgs)ee;
            if (e.id == MessagesToClientEnum.Logs)
            {
                Console.WriteLine("I know i got an Logs msg!");
                List<Log> logsList = JsonConvert.DeserializeObject<List<Log>>(e.msg);
                foreach (Log log in logsList)
                {
                    logs.Add(new LogModel { Type = log.Type.ToString(), Message = log.Message });

                }
                //Logs = logsList;
            }
            if (e.id == MessagesToClientEnum.Settings)
            {
                Console.WriteLine("I know i got an settings msg!");
               configModel.AddSettingsFromJson(e.msg);
            }

            if (e.id == MessagesToClientEnum.HandlerRemoved)
            {
                configModel.dirs.Remove(e.msg);
            }
        }

    }
}
