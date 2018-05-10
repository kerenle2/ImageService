using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<Model.MessageRecievedEventArgs> MessageRecieved;
        void Log(string message, Model.MessageTypeEnum type);           // Logging the Message

    }
}
