
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
using ImageService.Logging.Model;
using ImageService.Infrastructure.Enums;
using ImageService.Communication;
using ImageService.Infrastructure.CommandsInfrastructure;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private ServerTCP serverTCP;
        private IHandler configHandler;
        private IHandler logger_handler;
        private ConfigData configData;
        private string[] directories;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public List<IDirectoryHandler> Handlers;
        public event EventHandler<EventArgs> DataRecieved;
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        #endregion
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        public ImageServer(IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            this.Handlers = new List<IDirectoryHandler>();
            this.logger_handler = new LoggerHandler(m_logging, m_controller);
            this.configHandler = new ConfigHandler(this.m_controller);
            this.configData = ConfigData.InstanceConfig;
            this.directories = this.configData.Handlers;
            this.serverTCP = ServerTCP.getInstance();

            serverTCP.Start( );
            
            for (int i = 0; i < directories.Length; i++)
            {
                CreateHandler(directories[i], m_controller, m_logging);
            }

        }

        /// <summary>
        /// when the service should be closed, generates an event to 
        /// onCommandRecieved that says that services is closing
        /// </summary>
        public void Close()
        {            
            foreach (string dir in this.directories)
            {
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand,
                    null, dir);
                this.CommandRecieved?.Invoke(this, e);
            }
        }
        /// <summary>
        /// when the service stop listen to directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDirectoryClose(object sender, DirectoryCloseEventArgs e)
        {            
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            this.CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= this.OnDirectoryClose;
            this.configData.RemoveHandler(e.DirectoryPath);
            MsgInfoEventArgs msgI = new MsgInfoEventArgs(MessagesToClientEnum.HandlerRemoved, e.DirectoryPath);
            this.serverTCP.SendMsgToAll(this, msgI);
        }


        /// <summary>
        /// start listen to the directory
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        public void CreateHandler(string dir, IImageController controller, ILoggingService logger)
        {
            IDirectoryHandler handler = new DirectoryHandler(controller, dir, logger);
            this.Handlers.Add(handler);

            //start handler listening:
            this.serverTCP.ServerCommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += OnDirectoryClose;
            handler.StartHandleDirectory(dir);
        }
      



    }
}
