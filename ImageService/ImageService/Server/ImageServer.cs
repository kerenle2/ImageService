﻿
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using ImageService.Modal.Event;
using ImageService.Controller.Handlers;
using ImageService.Commands;
using System.Configuration;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public List<IDirectoryHandler> Handlers;
        #endregion

        public ImageServer(IImageController controller, ILoggingService logger)
        {

            Console.WriteLine("in server constructor");
            this.m_controller = controller;
            this.m_logging = logger;
            this.Handlers = new List<IDirectoryHandler>();
            
            string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            for (int i = 0; i < directories.Length; i++)
            {
                CreateHandler(directories[i], m_controller, m_logging);
            }
        }

        //what the server needs to do when service is closing:
        //generates an event to onCommandRecieved says that services is closing
        public void Close()
        {
            string[] dirs = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string dir in dirs)
            {
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand,
                    null, dir);
                this.CommandRecieved?.Invoke(this, e);
            }
        }

        public void OnDirectoryClose(object sender, DirectoryCloseEventArgs e)
        {
            //send msg to logger:
            string msg = e.Message;
            this.m_logging.Log(msg, MessageTypeEnum.INFO);

            //remove sender listening:
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            this.CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= this.OnDirectoryClose;

        }

        public void CreateHandler(string dir, IImageController controller, ILoggingService logger)
        {
            IDirectoryHandler handler = new DirectoryHandler(controller, dir, logger);
            this.Handlers.Add(handler);

            //start handler listening:
            this.CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += OnDirectoryClose;

           handler.StartHandleDirectory(dir); // now???
        }



    }
}
