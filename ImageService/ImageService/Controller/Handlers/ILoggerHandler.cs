using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Logging;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface ILoggerHandler
    {
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command
        void HandleSendLog(List<Log> listToSend);               //sends the list provided. if provided "null" than all the history list sent.
    }
}
