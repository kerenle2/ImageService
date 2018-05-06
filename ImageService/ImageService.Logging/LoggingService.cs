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
        /// <summary>
        /// add a message to the logger
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, Modal.MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));

        }
    }
}
