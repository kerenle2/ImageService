using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<Modal.MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, Modal.MessageTypeEnum type)
        {

        }
    }
}
