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
using ImageService.Logging.Model;
using System.Text.RegularExpressions;
using ImageService.Modal.Event;


namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="path"></param>
        /// <param name="logger"></param>
        public DirectoryHandler(IImageController controller, string path, ILoggingService logger)
        {
            m_controller = controller;
            m_path = path;
            m_logging = logger;

            createWatcher();
        }
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private string m_path;
        private FileSystemWatcher watcher;                  // The Watcher of the Dir
        // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed
        /// <summary>
        /// check what is the current command and run it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(this.m_path))
            {
                if(e.CommandID.Equals((int)CommandEnum.CloseCommand))
                {
                    m_logging.Log("Handler Recieved Close Command.", MessageTypeEnum.INFO);
                    handleCloseCommand();
                }
                else if (e.CommandID.Equals((int)CommandEnum.NewFileCommand))
                {
                    m_logging.Log("Handler Recieved New File Command.", MessageTypeEnum.INFO);
                    handleNewFile(e);
                }
            }
        }        
        /// <summary>
        /// create the watcher for the current dir
        /// </summary>
        public void createWatcher()
        {
           this.watcher = new FileSystemWatcher();
           watcher.Path = this.m_path;
            m_logging.Log("start watching the directory " + this.m_path, MessageTypeEnum.INFO);

            // watch all files in the directory.
            watcher.Filter = "*";

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            
        }

        
        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath"></param>
        void IDirectoryHandler.StartHandleDirectory(string dirPath)
        {
   
           
        }

        /// <summary>
        /// checks if the file changed is of types desired, and if so - raise a command.
        /// </summary>
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            FileInfo f = new FileInfo(e.FullPath);
            string[] args = {e.FullPath}; //or must be e.fullpath?
            string extension = Path.GetExtension(e.FullPath);

            //the extensions of ehr images that we should listen to.
            string[] extensions = { ".bmp", ".gif", ".png", ".jpg" };
            if (extensions.Contains(extension.ToLower()))
            {
                CommandRecievedEventArgs c = new CommandRecievedEventArgs(
                   (int)CommandEnum.NewFileCommand, args, m_path);
                this.OnCommandRecieved(this, c);

            }
            
        }

        /// <summary>
        /// handled the close command.
        /// </summary>
        void handleCloseCommand()
        {         
            string msg = "closing handler to path: " + m_path;
            this.m_logging.Log(msg, MessageTypeEnum.INFO);

            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(this.m_path, msg);

            watcher.EnableRaisingEvents = false;
            watcher.Created -= new FileSystemEventHandler(OnChanged);

            this.DirectoryClose?.Invoke(this, e); //sened an event back that notify the diretory is close (server will get this msg)

        }
        /// <summary>
        /// handle a new file in the dir
        /// </summary>
        /// <param name="e"></param>
        void handleNewFile(CommandRecievedEventArgs e)
        {
            Task addFileTask = new Task(() =>
            {
               string msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out bool result);
                if (result)
                {
                    this.m_logging.Log(msg, MessageTypeEnum.INFO);
                }
                else
                {
                    this.m_logging.Log(msg, MessageTypeEnum.FAIL);
                }
            });
            addFileTask.Start();

        }
    }


}
