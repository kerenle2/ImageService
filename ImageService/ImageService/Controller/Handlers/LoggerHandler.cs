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
        public ILoggingService logger;
        public IImageController controller;
        ServerTCP server = ServerTCP.getInstance();
        private List<Log> m_logList;
        LogHistory logHistory = LogHistory.getInstance();

        public LoggerHandler(ILoggingService m_logger, IImageController m_controller)
        {
            
            this.logger = m_logger;
            this.controller = m_controller;
            this.m_logList = logHistory.LogHistoryList;
            this.logger.MessageRecieved += AddToLoggerList;
        }

        public void AddToLoggerList (object sender, MessageRecievedEventArgs messageReceived)
        {
            this.logHistory.AddToLoggerList(new Log
            {
                Type = messageReceived.Status,
                Message = messageReceived.Message
            });
      
            HandleSendMessage(this.m_logList);
        }

    void HandleSendMessage(List<Log> list)
        {
            Task sendLogsTask = new Task(() =>
            {

                //  string listConveredToJson = JsonConvert.SerializeObject(list);
                  string[] args = new string[2];
             
                args[0] = ToJson();
                //CommandRecievedEventArgs ce = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, null);
               // bool result;
               // string msg = controller.ExecuteCommand(ce.CommandID, ce.Args , out result);
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, args[0]);
                
                server.SendMsgToAll(this, msgI); //maybe do it not that starit forword but throw notify all of the clientHandler latr.                                                                       
            });
            sendLogsTask.Start();
            //convert to jason and send to server
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(m_logList);
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
