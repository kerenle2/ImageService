using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Controller.Handlers;
using ImageService.Modal.Event;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller.Handlers
{
    public class LoggerHandler: ILoggerHandler
    {
        public ILoggingService logger;
        public IImageController controller;

        private List<Log> logList;
        public LoggerHandler(ILoggingService m_logger, IImageController m_controller)
        {
            
            this.logger = m_logger;
            this.controller = m_controller;
            this.logList = new List<Log> { };
            this.logger.MessageRecieved += AddToLoggerList;
        }
        public void AddToLoggerList (object sender, MessageRecievedEventArgs message)
        {
            Log log = new Log(message.Status, message.Message);
            this.logList.Add(log);
        }
        void sendMessage(CommandRecievedEventArgs e, List<Log> list)
        {
            Task sendLogsTask = new Task(() =>
            {
                bool result;
                string msg = controller.ExecuteCommand(e.CommandID, e.Args , out result);
               
            });
            sendLogsTask.Start();
            //convert to jason and send to server
        }
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {            
            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                sendMessage(e, this.logList); //// if its the command, send back to server the list. (but with jason)
            }
        }
    }
}
