using ImageService.Commands;
using ImageService.Communication;
using ImageService.Infrastructure;
using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Controller.Handlers;
using System.Net.Sockets;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private ServerTCP server = ServerTCP.getInstance();
        private IImageServiceModal m_modal;                      // The Modal Object
       // private LoggerHandler m_loggerHandler;
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal, ILoggingService logger)
        {
            m_modal = modal;                    // Storing the Modal Of The System
           
            commands = new Dictionary<int, ICommand>();
            commands.Add((int)CommandEnum.NewFileCommand, new AddFileCommand(m_modal));
            commands.Add((int)CommandEnum.LogCommand, new LogCommand());
            commands.Add((int)CommandEnum.GetConfigCommand, new GetConfigCommand());
            //add close command here


           
            server.DataRecieved += this.OnCommandRecieved;
            server.newClientConnected += this.OnNewClientConnected;

        }
        /// <summary>
        /// execute the current command
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="resultSuccesful"></param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                resultSuccesful = true;
                ICommand c = commands[commandID];
                return c.Execute(args, out resultSuccesful);
            }
            else
            {
                resultSuccesful = false;
                string msg = "The Command ID given does not exist";
                return msg;
            }

        }

        public void OnNewClientConnected(object sender, TcpClient client)
        {
            TcpClient c = new TcpClient();
          //  this.m_loggerHandler.HandleSendLogsList(client);                //LOGGER HANDLER HERE IS NULL
            //add here also send appConfig
        }

        public void OnCommandRecieved(object sender, EventArgs e)
        {
            CommandRecievedEventArgs c = (CommandRecievedEventArgs)e;
            int commandId = c.CommandID;
            string[] args = c.Args;

            //add here handling remove handler!!!!! not the same as close command... use c.RequestDirPath

            bool resault;
            ExecuteCommand(commandId, args, out resault); //resault will cmoe back here and were not using it... what to do?
        }
    }
}
