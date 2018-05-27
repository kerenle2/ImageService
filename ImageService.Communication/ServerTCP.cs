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
        //members
        private int port;
        private TcpListener listener;
        private List<TcpClient> clientsList;
        private static ServerTCP instance = null;

        public event EventHandler<CommandRecievedEventArgs> ServerCommandRecieved;
        public event EventHandler<EventArgs> DataRecieved;
        public event EventHandler<RequestDataEventArgs> NewClientConnected;

        Mutex readLock = new Mutex();
        Mutex writeLock = new Mutex();

        /// <summary>
        /// returns instance of the server - Singelton Pattern
        /// </summary>
        /// <returns></returns>
        public static ServerTCP getInstance()
        {
            if (instance == null)
            {
                instance = new ServerTCP();
            }
            return instance;
        }

        /// <summary>
        /// private constructor - Singelton Pattern
        /// </summary>
        private ServerTCP()
        {
            this.clientsList = new List<TcpClient>();
            this.port = 8000;

        }

        /// <summary>
        /// sends a msg to the specfied client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msgI"></param>
        /// <param name="client"></param>
        public void SendMsgToOneClient(object sender, MsgInfoEventArgs msgI, TcpClient client)
        {
            new Task(() =>
            {
                try
                {
                    this.writeLock.WaitOne();
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    string msg = JsonConvert.SerializeObject(msgI);
                    writer.Write(msg);
                    this.writeLock.ReleaseMutex();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }).Start();
        }


        /// <summary>
        /// sends msg to all the clients connected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msgI"></param>
        public void SendMsgToAll(object sender, MsgInfoEventArgs msgI)
        {
       
            foreach (TcpClient client in this.clientsList)
            {
                SendMsgToOneClient(sender, msgI, client);   
            }
               
  
           
        }

        
        /// <summary>
        /// starts the server activity - initialize components and
        /// wait for clients connection request in a new thread
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("server: Waiting for connections...");
            //Thread.Sleep(1000);

            ListenToClients();

        }

        /// <summary>
        /// listen to clients connection reuests in a new thread.
        /// when client connected - handle his requests.
        /// </summary>
        public void ListenToClients()
        {
            Task waitForClientsConnections = new Task(() =>
            {
                while (true)
                {
                    try
                    {

                        TcpClient client = listener.AcceptTcpClient();
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


        /// <summary>
        /// gets the clients request and invokes the apropriate event.
        /// </summary>
        /// <param name="client"></param>
        public void HandleClient(TcpClient client)
        {
            Task handleClientRequest = new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                while(client.Connected)
                {
                    try
                    {
                    string commandLine = reader.ReadString();
                        Console.WriteLine("Got command: {0}", commandLine);

                        //handle the command:
                        CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        if(command.CommandID == ((int)CommandEnum.CloseCommand))
                        {
                            ServerCommandRecieved?.Invoke(this, command);
                        }
                        else
                        {
                            DataRecieved?.Invoke(this, command);
                        }

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
