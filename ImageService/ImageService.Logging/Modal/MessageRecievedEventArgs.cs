using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs: EventArgs
    {
        public MessageRecievedEventArgs(string message, Modal.MessageTypeEnum status)
        {
            this.Message = message;
            this.Status = status;
        }
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
