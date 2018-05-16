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
   //     void OnCommandRecieved(object sender, EventArgs e);
        void SendToServer(MsgInfoEventArgs msgI, TcpClient client = null);
        event EventHandler<RequestDataEventArgs> RequestData;
    }
}
