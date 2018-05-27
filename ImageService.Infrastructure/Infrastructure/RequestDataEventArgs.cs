using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.CommandsInfrastructure
{
    public class RequestDataEventArgs : EventArgs
    {
        private TcpClient m_client;
        public TcpClient client
        {
            get { return m_client; }
        }

        //constructor
        public RequestDataEventArgs(TcpClient client)
        {
            this.m_client = client;
        }
    }
}
