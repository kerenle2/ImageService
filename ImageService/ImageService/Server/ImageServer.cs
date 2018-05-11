
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

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private ServerTCP serverTCP;
        private IClientHandler client_handler;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public List<IDirectoryHandler> Handlers;
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
            this.client_handler = new ClientHandler();
            string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            this.serverTCP = new ServerTCP(8000, this.client_handler);
            serverTCP.Start( );
            for (int i = 0; i < directories.Length; i++)
            {
                CreateHandler(directories[i], m_controller, m_logging);
            }
          //  toDebudDeletaLater();
        }

        public void toDebudDeletaLater()
        {
            Client c = Client.getInstance();
            
            MsgInfo msgI = new MsgInfo(MessagesToClientEnum.HandlerRemoved, "Handler");
            this.serverTCP.sendMsg(c.getMyTcpClient(), msgI);

        }
        /// <summary>
        /// when the service should be closed, generates an event to 
        /// onCommandRecieved that says that services is closing
        /// </summary>
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
        /// <summary>
        /// when the service stop listen to directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            this.CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += OnDirectoryClose;

           handler.StartHandleDirectory(dir); // now???
        }



    }
}
