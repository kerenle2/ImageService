using ImageService.Commands;
using ImageService.Communication;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            commands.Add((int)CommandEnum.NewFileCommand, new AddFileCommand(m_modal));
            commands.Add((int)CommandEnum.LogCommand, new LogCommand());


            //check:
            ServerTCP server = ServerTCP.getInstance();

            //add close command here
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
    }
}
