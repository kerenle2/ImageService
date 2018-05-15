using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;

namespace ImageService.Logging
{
    public class Log
    {
        public MessageTypeEnum Type { get; set; }
        public string Message { get; set; }
    }
}

