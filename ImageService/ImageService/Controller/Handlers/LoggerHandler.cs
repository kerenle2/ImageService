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
        public ILoggingService logger;
        public IImageController controller;
        private List<Log> m_logList;
        Mutex listLock = new Mutex();
        LogHistory logHistory = LogHistory.getInstance();

        public LoggerHandler(ILoggingService m_logger, IImageController m_controller)
        {

            this.logger = m_logger;
            this.controller = m_controller;
            this.m_logList = logHistory.LogHistoryList;
            this.logger.MessageRecieved += AddToLoggerList;
            this.logger.MessageRecieved += SendOneLog;

            controller.RequestData += OnRequestData;
        }

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

        //public void HandleSendLog(List<Log> listToSend)
        //{
        //    Task sendLogsTask = new Task(() =>
        //    {
        //        if (listToSend == null)
        //        {
        //            listToSend = new List<Log>();
        //            listLock.WaitOne();
        //            listToSend = m_logList;
        //            listLock.ReleaseMutex();
        //        }
        //        string JsonList = JsonConvert.SerializeObject(listToSend);
        //        MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, JsonList);
        //        controller.SendToServer(msgI);
        //    });
        //    sendLogsTask.Start();
        //}


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

        //public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        //{
        //    if (e.CommandID.Equals((int)CommandEnum.LogCommand))
        //    {
        //        listLock.WaitOne();
        //        HandleSendLog(this.m_logList);
        //        Thread.Sleep(150);
        //        listLock.ReleaseMutex();
        //    }
        //}



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

            //send only this message to the client:
            //List<Log> list = new List<Log>();
            //list.Add(l);
            //HandleSendLog(list);
            
        }

        public void OnRequestData(object sender, RequestDataEventArgs e)
        {
            HandleSendLogsRequest(e);
        }

    }

    

}

