using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.CommandsInfrastructure
{
    public class ConfigData
    {
        private static ConfigData configData;


        public string OutputDir { get; set; }
        public string EventSourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public string[] Handlers { get; set; }
        /// <summary>
        /// constructor. take all the data from app config.
        /// </summary>
        private ConfigData()
        {
            this.OutputDir = ConfigurationManager.AppSettings.Get("OuptputDir");
            this.ThumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            this.EventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            this.LogName = ConfigurationManager.AppSettings.Get("LogName");
            this.Handlers = ConfigurationManager.AppSettings.Get("Handler").Split(';');

        }
        public static ConfigData InstanceConfig
        {
            get
            {
                if (configData == null)
                {
                    configData = new ConfigData();
                }
                return configData;
            }
        }
        public void RemoveHandler(string path)
        {
            for (int i = 0; i < Handlers.Length; i++)
            {
                if (Handlers[i].Equals(path))
                {
                    var listHandlers = new List<string>(Handlers);
                    listHandlers.RemoveAt(i);
                    this.Handlers = listHandlers.ToArray();
                }
            }

        }
        public string ToJSON()
        {
            JObject configJson = new JObject();
            configJson["Handlers"] = JToken.FromObject(Handlers);
            configJson["OutputDir"] = OutputDir;
            configJson["EventSourceName"] = EventSourceName;
            configJson["LogName"] = LogName;
            configJson["ThumbnailSize"] = ThumbnailSize;
            return configJson.ToString();

        }

        //public void FromJson(string str)
        //{
        //    JObject configJson = JObject.Parse(str);
        //    Handlers = (configJson["Handlers"]).ToObject<string[]>();
        //    LogName = (string)configJson["LogName"];
        //    EventSourceName = (string)configJson["SourceName"];
        //    OutputDir = (string)configJson["OutputDir"];
        //    ThumbnailSize = (int)configJson["ThumbnailSize"];

        //}
    }
}
