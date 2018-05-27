using ImageService.Communication;
using ImageService.Infrastructure.CommandsInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
        void SendToServer(MsgInfoEventArgs msgI, TcpClient client = null);             // ask server to forword this msgI to specific client if specified, 
                                                                                       //or to all clients if not specified. 
        event EventHandler<RequestDataEventArgs> RequestData;                          //the event that will be activated when server needs data To send
    }
}
