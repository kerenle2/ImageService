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

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private Dictionary<string, ICommand> commands;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public List<IDirectoryHandler> Handlers;
        #endregion

        public ImageServer()
        {
            this.Handlers = new List<IDirectoryHandler>();
            string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            for (int i = 0; i < directories.Length; i++)
            {
                CreateHandler(directories[i], m_controller);
            }
        }


        public void CreateHandler(string dir, IImageController controller)
        {
            IDirectoryHandler handler = new DirectoryHandler(dir, controller);
            this.Handlers.Add(handler);
            this.CommandRecieved += handler.OnCommandRecieved;
        

           // h.onClose += onCloseServer} - from haviva
            handler.StartHandleDirectory(dir); // now???
        }
    }
}
