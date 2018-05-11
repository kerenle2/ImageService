using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    
    public class ServerTCP
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private List<TcpClient> clientsList;

        //private static ServerTCP instanceServer;

        //private ServerTCP() { }
        public ServerTCP(int port, IClientHandler ch)
        {
            this.clientsList = new List<TcpClient>();
            this.port = port;
            this.ch = ch;

        }

        //public static ServerTCP Instance
        //{
        //    get
        //    {
        //        if (instanceServer == null)
        //        {
        //            instanceServer = new ServerTCP();
        //        }
        //        return instanceServer;
        //    }
        //}

        public void sendMsg(TcpClient client, MsgInfo msgI)
        {

            ch.SendMsg(client, msgI);
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("server: Waiting for connections...");

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");

                        this.clientsList.Add(client);
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }


    }
    
}
