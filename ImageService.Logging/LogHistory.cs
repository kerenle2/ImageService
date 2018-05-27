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

        /// <summary>
        /// private constructor - Singelton pattern
        /// </summary>
        private LogHistory()
        {
            LogHistoryList = new List<Log>();
        }

        /// <summary>
        /// returns instance - Singelton pattern
        /// </summary>
        /// <returns></returns>
        public static LogHistory getInstance()
        {
            if (instance == null)
            {
                instance = new LogHistory();
            }
            return instance;
        }


        /// <summary>
        /// adds a log to the logs history list
        /// </summary>
        /// <param name="log"></param>
        public void AddToLoggerList(Log log)
        {
            this.LogHistoryList.Add(log);   
        }
        
    }
}
