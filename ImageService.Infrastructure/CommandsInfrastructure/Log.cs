using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.CommandsInfrastructure
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
