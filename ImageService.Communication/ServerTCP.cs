using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ImageService.Communication
{
    
    public class ServerTCP : ICommunicate
    {
        private int port;
        private TcpListener listener;
        private List<TcpClient> clientsList;
        private NetworkStream stream;


        private static ServerTCP instance = null;

        //public event EventHandler<CommandRecievedEventArgs> ServerCommandRecieved;
        public event EventHandler<EventArgs> DataRecieved;
        public event EventHandler<RequestDataEventArgs> NewClientConnected;

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

        }


        public void SendMsgToOneClient(object sender, MsgInfoEventArgs msgI, TcpClient client)
        {
            new Task(() =>
            {
                try
                {
                    this.stream = client.GetStream();

                    BinaryWriter writer = new BinaryWriter(stream);
                    {
                        string msg = JsonConvert.SerializeObject(msgI);
                        writer.Write(msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }).Start();
        }

        public void SendMsgToAll(object sender, MsgInfoEventArgs msgI)
        {
       
            foreach (TcpClient client in this.clientsList)
            {
                SendMsgToOneClient(sender, msgI, client);   
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
            Task waitForClientsConnections = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Thread.Sleep(1000);
                        Console.WriteLine("Got new connection");
                        RequestDataEventArgs e = new RequestDataEventArgs(client);
                        NewClientConnected?.Invoke(this, e);
                        this.clientsList.Add(client);
                        HandleClient(client);

                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            waitForClientsConnections.Start();
        }


        public void HandleClient(TcpClient client)
        {
            Task handleClientRequest = new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryReader writer = new BinaryReader(stream))
                {
                    try
                    {
                        //send logs history list:
                        
                  
                        string commandLine = reader.ReadString();
                        Console.WriteLine("Got command: {0}", commandLine);
                        //    string result = m_controller.ExecuteCommand(commandLine, client);
                        //string result;

                        //handle the command:
                        CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        DataRecieved?.Invoke(this, command);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Server Error: " + e.StackTrace);
                    }
                }
            }); handleClientRequest.Start();
        }

        public void Stop()
        {
            listener.Stop();
        }

    }
    
}
