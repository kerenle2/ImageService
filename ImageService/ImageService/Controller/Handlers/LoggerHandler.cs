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
using System.Threading;
using System.Net.Sockets;

namespace ImageService.Controller.Handlers
{
    public class LoggerHandler : IHandler
    {
        //members
        public ILoggingService logger;
        public IImageController controller;
        private List<Log> m_logList;
        Mutex listLock = new Mutex();
        LogHistory logHistory = LogHistory.getInstance();

        //constructor
        public LoggerHandler(ILoggingService m_logger, IImageController m_controller)
        {

            this.logger = m_logger;
            this.controller = m_controller;
            this.m_logList = logHistory.LogHistoryList;
            this.logger.MessageRecieved += AddToLoggerList;
            this.logger.MessageRecieved += SendOneLog;

            controller.RequestData += OnRequestData;
        }

        /// <summary>
        /// sends controller the logs list in a new thread
        /// </summary>
        /// <param name="e"></param>
        public void HandleSendLogsRequest(RequestDataEventArgs e)
        {
            Task sendLogsTask = new Task(() =>
            {
                string JsonList = JsonConvert.SerializeObject(m_logList);
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, JsonList);
                controller.SendToServer(msgI, e.client);
            });
            sendLogsTask.Start();
        }

        /// <summary>
        /// sends one log msg in a new thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SendOneLog(object sender, MessageRecievedEventArgs e)
        {
            Task sendLogsTask = new Task(() =>
            {
                Log l = new Log
                {
                    Type = e.Status,
                    Message = e.Message
                };
                List<Log> list = new List<Log>();
                list.Add(l);

                string JsonList = JsonConvert.SerializeObject(list);
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, JsonList);
                controller.SendToServer(msgI);
            });
            sendLogsTask.Start();
        }

        /// <summary>
        /// when msg recived, add it to the loggerList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageReceived"></param>
        public void AddToLoggerList(object sender, MessageRecievedEventArgs messageReceived)
        {
            Log l = new Log
            {
                Type = messageReceived.Status,
                Message = messageReceived.Message
            };

            listLock.WaitOne();
            this.logHistory.AddToLoggerList(l);
            listLock.ReleaseMutex();            
        }

        /// <summary>
        /// activated when controller asks for data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRequestData(object sender, RequestDataEventArgs e)
        {
            HandleSendLogsRequest(e);
        }

    }

    

}

