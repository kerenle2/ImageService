using Newtonsoft.Json;
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
        private IClientHandler client_handler;
        private List<TcpClient> clientsList;
        private Mutex readMutex;
        private Mutex writeMutex;

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

            writeMutex = new Mutex();
            readMutex = new Mutex();
            Thread.Sleep(1000);
        }



        public void SendMsgToAll(object sender, MsgInfoEventArgs msgI)
        {
       
                foreach (TcpClient client in this.clientsList)
                {

                NetworkStream stream = client.GetStream();
                        StreamWriter writer = new StreamWriter(stream);

                         writeMutex.WaitOne();
                         string msg = JsonConvert.SerializeObject(msgI);
                    //   string msg = "hhh";

                    try
                    {
                        writer.Write(msg);

                    } catch(Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }

                       writeMutex.ReleaseMutex();
            }
             
        }

  

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("server: Waiting for connections...");
            Task listen = new Task(() =>
            {
                ListenToClients();
            });
            listen.Start();

        }

        public void ListenToClients()
        {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");

                        this.clientsList.Add(client);
                        Task handleTheClientAccepted = new Task(() =>
                        {
                            //add listen to commands
                            IClientHandler clientHandler = new ClientHandler();
                            clientHandler.HandleClient(client);
                        });
                        handleTheClientAccepted.Start();

                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");

        }
        public void Stop()
        {
            listener.Stop();
        }

    }
    
}
