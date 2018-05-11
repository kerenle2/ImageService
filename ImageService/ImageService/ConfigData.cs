using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class ConfigData
    {
        private static ConfigData configData;
        
       
        public string OutputDir { get; }
        public string EventSourceName { get; }
        public string LogName { get; }
        public int ThumbnailSize { get; }
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
    }
}
