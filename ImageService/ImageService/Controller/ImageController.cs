﻿using ImageService.Commands;
using ImageService.Communication;
using ImageService.Infrastructure;
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
        private Dictionary<int, ICommand> commands;

        public event EventHandler<RequestDataEventArgs> RequestData;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal, ILoggingService logger)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            commands.Add((int)CommandEnum.NewFileCommand, new AddFileCommand(m_modal));

            server.DataRecieved += this.OnCommandRecieved;
            server.NewClientConnected += this.OnNewClientConnected;

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
        /// <summary>
        /// asks the server to send this msg info to all clients, or to a specific client if specified
        /// </summary>
        /// <param name="msgI"></param> the msg info to send
        /// <param name="client"></param> if not specified, the server will send this msg to All his clients
        public void SendToServer(MsgInfoEventArgs msgI, TcpClient client = null)
        {
            if (client == null)
            {
                server.SendMsgToAll(this, msgI);
            }
            else
            {
                server.SendMsgToOneClient(this, msgI, client);
            }
        }

        /// <summary>
        /// the event that will be activated upon new client connected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNewClientConnected(object sender,RequestDataEventArgs e)
        {
            RequestData?.Invoke(this, e);
        }


        public void OnCommandRecieved(object sender, EventArgs e)
        {

            CommandRecievedEventArgs c = (CommandRecievedEventArgs)e;
            int commandId = c.CommandID;
            string[] args = c.Args;

            bool resault;
            ExecuteCommand(commandId, args, out resault); //resault will cmoe back here and were not using it... what to do?
        }
    }
}
