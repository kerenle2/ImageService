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
using ImageService.Commands;
using ImageService.Communication;
using Newtonsoft.Json;

namespace ImageService.Controller.Handlers
{
    public class LoggerHandler: ILoggerHandler
    {
        public ILoggingService logger;
        public IImageController controller;
        ServerTCP server = ServerTCP.getInstance();
        private List<Log> m_logList;


        public LoggerHandler(ILoggingService m_logger, IImageController m_controller)
        {
            
            this.logger = m_logger;
            this.controller = m_controller;
            this.m_logList = new List<Log> { };
            this.logger.MessageRecieved += AddToLoggerList;
        }

        public void AddToLoggerList (object sender, MessageRecievedEventArgs message)
        {
            Log log = new Log(message.Status, message.Message);
            this.m_logList.Add(log);
            HandleSendMessage(this.m_logList);
        }

    void HandleSendMessage(List<Log> list)
        {
            Task sendLogsTask = new Task(() =>
            {
                
                string listConveredToJson = JsonConvert.SerializeObject(list);
                string[] args = new string[2];
                args[0] = listConveredToJson;
                CommandRecievedEventArgs ce = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, null);
                bool result;
                string msg = controller.ExecuteCommand(ce.CommandID, ce.Args , out result);
                                                                                         
            });
            sendLogsTask.Start();
            //convert to jason and send to server
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {            
            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                HandleSendMessage(this.m_logList); //// if its the command, send back to server the list. (but with jason)
            }
        }
    }
}
