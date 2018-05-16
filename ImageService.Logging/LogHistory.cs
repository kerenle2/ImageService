using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LogHistory
    {
        private static LogHistory instance = null;
        public List<Log> LogHistoryList { get; set; }
        private LogHistory()
        {
            LogHistoryList = new List<Log>();
        }
        public static LogHistory getInstance()
        {
            if (instance == null)
            {
                instance = new LogHistory();
            }
            return instance;
        }
        public void AddToLoggerList(Log log)
        {
            this.LogHistoryList.Add(log);   
        }
        
    }
}
