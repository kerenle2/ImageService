using ImageService.Controller.Handlers;
using Newtonsoft.Json;
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
       //     this.logList = logList;
        }
        public string Execute(string[] args, out bool result)
        {
            
            string JsonConvertedList = args[0];
            result = true;
            return JsonConvertedList; //ready to sent to server

        }
    }
}
