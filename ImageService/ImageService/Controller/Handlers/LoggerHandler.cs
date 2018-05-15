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
using ImageService.Infrastructure.CommandsInfrastructure;

namespace ImageService.Controller.Handlers
{
    public class LoggerHandler: ILoggerHandler
    {
        IClientHandler ch = new ClientHandler();
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
            HandleLogsSending();
        }

    public void HandleLogsSending()
        {
           // Task sendLogsTask = new Task(() =>
            //{

               string msg = "blabla enter here the real list as string - NOT AS JASON YET";
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, msg);
                server.SendMsgToAll(this, msgI); //maybe do it not that starit forword but throw notify all of the clientHandler latr. 
               
          //  });
           // sendLogsTask.Start();
            //convert to jason and send to server
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {            
            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                HandleLogsSending(); //// if its the command, send back to server the list. (but with jason)
            }
        }

  
    }
}
