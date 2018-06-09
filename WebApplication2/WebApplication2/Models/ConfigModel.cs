using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace WebApplication2.Models
{
    public class ConfigModel
    {
        public ConfigModel()
        {
            this.dirs = new List<string>();
            //maybe sleep?
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory")]
        public string outputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string sourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string logName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size")]
        public string thumbSize { get; set; }


        public List<string> dirs { get; set; }

        /// <summary>
        /// convert back the json string and update the data.
        /// </summary>
        /// <param name="str"></param>
        public void AddSettingsFromJson(string str)
        {
            JObject configJson = JObject.Parse(str);
            List<string> handlers = (configJson["Handlers"]).ToObject<List<string>>();
            foreach (string handler in handlers)
            {
               this.dirs.Add(handler);
            }
            string LogName = configJson["LogName"].ToObject<string>();
            logName = LogName;
            this.sourceName = (string)configJson["EventSourceName"];
            this.outputDir = (string)configJson["OutputDir"];
            this.thumbSize = ((int)configJson["ThumbnailSize"]).ToString();
        }
    }
}