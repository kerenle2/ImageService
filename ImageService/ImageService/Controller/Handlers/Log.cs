using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;
namespace ImageService.Controller.Handlers
{
    public class Log
    {
        private MessageTypeEnum type;
        private string message;
        public Log(MessageTypeEnum type, string message)
        {
            this.type = type;
            this.message = message;
        }
    }
}
