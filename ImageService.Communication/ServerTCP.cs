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
        private IClientHandler client_handler;
        private List<TcpClient> clientsList;
        private NetworkStream stream;

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

        }



        public void SendMsgToAll(object sender, MsgInfoEventArgs msgI)
        {
       
                foreach (TcpClient client in this.clientsList)
                {
                new Task(() =>
                {
                    try
                    {
                        this.stream = client.GetStream();

                       // BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);
                        {
                             string msg = JsonConvert.SerializeObject(msgI);
                            //ToJson((int)msgI.id, msgI.msg);
                            
                            writer.Write(msg);

                        }

                       // client.Close();
                    } catch(Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
               
                }).Start();
            }
               
  
           
        }

        public string ToJson(int id, string msg)
        {
            JObject messageObj = new JObject();
            messageObj["TypeMessage"] = id;
            messageObj["Content"] = msg;
            return messageObj.ToString();
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
                    //    System.Threading.Thread.Sleep(200);

                        this.clientsList.Add(client);
                        client_handler.HandleClient(client);

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
