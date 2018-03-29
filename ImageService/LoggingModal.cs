using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    
    class LoggingModal
    {
        public LoggingModal(string s)
        {
            Text = s;
        }
        public String Text
        {
            get;
            set;
        }
    }
}
