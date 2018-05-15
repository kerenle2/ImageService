//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ImageService.Infrastructure.CommandsInfrastructure
//{
//    public class LogsConvertor
//    {
//        public LogsConvertor() { }

//        public string ToJSON(List<Log> list)
//        {
//            return JsonConvert.SerializeObject(Logs, Formatting.Indented);

//            JObject configJson = new JObject();
//            foreach(Log log in list)
//            {
//                configJson["log]
//            }
//            configJson["Handlers"] = JToken.FromObject(Handlers);
//            configJson["OutputDir"] = OutputDir;
//            configJson["EventSourceName"] = EventSourceName;
//            configJson["LogName"] = LogName;
//            configJson["ThumbnailSize"] = ThumbnailSize;
//            return configJson.ToString();

//        }

//        public List<Log> FromJson(string str)
//        {
//            JObject configJson = JObject.Parse(str);
//            Handlers = (configJson["Handlers"]).ToObject<string[]>();
//            LogName = (string)configJson["LogName"];
//            EventSourceName = (string)configJson["SourceName"];
//            OutputDir = (string)configJson["OutputDir"];
//            ThumbnailSize = (int)configJson["ThumbnailSize"];

//        }
//    }
//}
