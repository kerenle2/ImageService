using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    interface IHandler
    {
        void OnRequestData(object sender, RequestDataEventArgs e);      // The Event that will be activated upon new Request
    }
}
