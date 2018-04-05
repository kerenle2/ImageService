using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        public DirectoryHandler(string path, IImageController controller)
        {
            this.path = path;
            this.controller = controller;
        }
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        string path;
        IImageController controller;

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        void IDirectoryHandler.StartHandleDirectory(string dirPath)
        {
            // The Function Recieves the directory to Handle
        }
        void IDirectoryHandler.OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
        }        

        
    }
}
