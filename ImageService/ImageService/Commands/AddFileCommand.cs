using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class AddFileCommand: ICommand
    {
        private IImageServiceModal m_modal;

        public AddFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
         /// <summary>
         /// execute this coomand.
         /// </summary>
         /// <param name="args"></param>
         /// <param name="result"></param>
         /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, else will return the error message
              string path = args[0];
     
            return m_modal.AddFile(path, out result);
        }
    }
    }
