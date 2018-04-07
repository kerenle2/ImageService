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

  
        public DirectoryHandler(IImageController controller, string path)
        {

            m_controller = controller;
            m_path = path;
            createWatcher();


        }
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private string m_path;
        private FileSystemWatcher watcher;                  // The Watcher of the Dir
        // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed





        void IDirectoryHandler.OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(this.m_path))
            {
                if(e.CommandID.Equals(CommandEnum.CloseCommand))
                {
                    handleCloseCommand();
                }
                else if (e.CommandID.Equals(CommandEnum.NewFileCommand))
                {
                    handleNewFile(e);
                }
            }
        }        

        public void createWatcher()
        {
           this.watcher = new FileSystemWatcher();
           watcher.Path = this.m_path;

            // watch all files in the directory.
            watcher.Filter = "*";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;


        }

        // The Function Recieves the directory to Handle
        void IDirectoryHandler.StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            createWatcher();
        }

        //checks if the file changed is of types desired, and if so - raise a command.
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            FileInfo f = new FileInfo(e.FullPath);
            string[] args = {e.FullPath}; //or must be e.fullpath?
            string ending = Path.GetExtension(e.FullPath);


            string[] endings = { ".bmp", ".gif", ".png", ".jpg" };
            if (endings.Contains(ending.ToLower()))
            {
                CommandRecievedEventArgs c = new CommandRecievedEventArgs(
                   (int)CommandEnum.NewFileCommand, args, m_path);
            }


                if (f.Extension.Equals(".jpg") || f.Extension.Equals(".png")
                || f.Extension.Equals(".gif") || f.Extension.Equals(".bmp"))
            {
                CommandRecievedEventArgs c = new CommandRecievedEventArgs(
                    (int)CommandEnum.NewFileCommand, args, m_path);
            }
        }

        void handleCloseCommand()
        {         
            string msg = "closing handler to path: " + m_path;
            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(this.m_path, msg);
            watcher.EnableRaisingEvents = false;

            watcher.Changed -= new FileSystemEventHandler(OnChanged);
            watcher.Created -= new FileSystemEventHandler(OnChanged);

            //add logger msg here...
        }

        void handleNewFile(CommandRecievedEventArgs e)
        {
            Task addFileTask = new Task(() =>
            {
               bool result;
               string msg = m_controller.ExecuteCommand(e.CommandID, e.Args/*why args and not only path?*/, out result);

                //add logging msg according to result

            });

            addFileTask.Start();
        }


    }


}
