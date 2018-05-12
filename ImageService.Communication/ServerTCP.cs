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
    
    public class ServerTCP : ICommunicate
    {
        private int port;
        private TcpListener listener;
        private IClientHandler client_handler;
        private List<TcpClient> clientsList;

        private static ServerTCP instance = null;

        public event EventHandler<MsgInfoEventArgs> DataRecieved;

        public static ServerTCP getInstance()
        {
            if (instance == null)
            {
                instance = new ServerTCP();
            }
            return instance;
        }

        private ServerTCP() //changed to singelton, need to make sure it works.
        {
            this.clientsList = new List<TcpClient>();
            this.port = 8000;
            this.client_handler = new ClientHandler();
            client_handler.NotifyAllClients += SendMsgToAll;

        }


      
        public void SendMsgToAll(object sender, MsgInfoEventArgs msgI)
        {
       
                foreach (TcpClient client in this.clientsList)
                {
                new Task(() =>
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string msg = JsonConvert.SerializeObject(msgI);
                        writer.Write(msg);
                        //  Console.WriteLine("Got command: {0}", commandLine);
                        //string result = m_controller.ExecuteCommand(commandLine, client);
                        //writer.Write(result);
                    }

                    client.Close();
                }).Start();
            }
  
           
        }

  

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("server: Waiting for connections...");
            ListenToClients();

        }

        public void ListenToClients()
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");

                        this.clientsList.Add(client);
                        client_handler.HandleClient(client);

                        //here or inside handle - invoke with the resault
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
