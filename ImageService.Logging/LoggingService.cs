using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;

namespace ImageService.Logging
{
    public class LoggingService: ILoggingService
    { 
        
        public event EventHandler<Model.MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// add a message to the logger
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, Model.MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
            //add here invoke to an event that will invoke notifyAllClients
        }
    }
}
