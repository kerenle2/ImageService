using ImageService.Controller.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class LogCommand: ICommand
    {
        private List<Log> logList;
        public LogCommand()
        {
            this.logList = null;//??????????mybe not necessary
        }
        public string Execute(string[] args, out bool result)
        {
            string path = args[0];
            result = false; //deleteeeeeeee
            return "bdflb"; //send list to client here!!!!!!!1

        }
    }
}
