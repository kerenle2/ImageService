
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


        public void CreateHandler(string dir, IImageController controller, ILoggingService logger)
        {
            IDirectoryHandler handler = new DirectoryHandler(controller, dir, logger);
            this.Handlers.Add(handler);
            this.CommandRecieved += handler.OnCommandRecieved;
        
           // h.onClose += onCloseServer} - from haviva
           handler.StartHandleDirectory(dir); // now???
        }

        //change this - copied:
        //public void createCommand(int CommandID, string[] Args, string RequestDirPath)
        //{
        //    m_logging.Log("In create command", MessageTypeEnum.INFO);
        //    CommandRecievedEventArgs closeCommandArgs = new CommandRecievedEventArgs(
        //        CommandID, Args, RequestDirPath);
        //    this.CommandRecieved?.Invoke(this, closeCommandArgs);

        //}


    }
}
