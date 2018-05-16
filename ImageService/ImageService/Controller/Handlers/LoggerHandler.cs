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
    public class LoggerHandler: ILoggerHandler
    {
        public ILoggingService logger;
        ServerTCP server = ServerTCP.getInstance();
        private List<Log> m_logList;
        Mutex listLock = new Mutex();


        public LoggerHandler(ILoggingService m_logger)
        {

            this.logger = m_logger;
            this.m_logList = new List<Log>();
            this.logger.MessageRecieved += AddToLoggerList;
        }

        public void AddToLoggerList (object sender, MessageRecievedEventArgs messageReceived)
        {
            Log l = new Log
            {
                Type = messageReceived.Status,
                Message = messageReceived.Message
            };

            listLock.WaitOne();
            this.m_logList.Add(l);
            listLock.ReleaseMutex();
            //this.m_logList.Add(new Log
            //{
            //    Type = messageReceived.Status,
            //    Message = messageReceived.Message
            //});

            //send oonly this message to the client:
            List<Log> list = new List<Log>();
            list.Add(l);
            HandleSendLog(list);
        }
        public List<Log> getLogsList()
        {
            listLock.WaitOne();
            List<Log> l = this.m_logList;
            listLock.ReleaseMutex();
            return l;

        }

        public void HandleSendLogsList(TcpClient client)
        {
            Task sendLogsTask = new Task(() =>
            {

                string JsonList = JsonConvert.SerializeObject(m_logList);
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, JsonList);
                server.SendMsgToOneClient(this, msgI, client);
            });
            sendLogsTask.Start();
        }

        public void HandleSendLog(List<Log> listToSend)
        {
            Task sendLogsTask = new Task(() =>
            {
                if(listToSend == null)
                {
                    listToSend = new List<Log>();
                    listLock.WaitOne();
                    listToSend = m_logList;
                    listLock.ReleaseMutex();
                }
                string JsonList = JsonConvert.SerializeObject(listToSend);
                MsgInfoEventArgs msgI = new MsgInfoEventArgs((int)MessagesToClientEnum.Logs, JsonList);
                server.SendMsgToAll(this, msgI);                                                                       
            });
            sendLogsTask.Start();
        }


        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {            
            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                listLock.WaitOne();
                HandleSendLog(this.m_logList);
                Thread.Sleep(150);
                listLock.ReleaseMutex();
            }
        }

  
    }
}
