using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class Client : ICommunicate
    {
        //members
        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;
        Mutex readLock = new Mutex();
        Mutex writeLock = new Mutex();
        public bool Conected { get; set; }
        private NetworkStream stream = null;
        private static Client instance = null;
        public event EventHandler<EventArgs> DataRecieved;

        /// <summary>
        /// returns the instance of client (singelton)
        /// </summary>
        /// <returns></returns>
        public static Client getInstance()
        {
            if (instance == null)
            {
                instance = new Client();
            }
            return instance;
        }



        /// <summary>
        /// private constructor - singelton Pattern
        /// </summary>
        private Client()
        {
            
            Console.WriteLine("client: in constructor");
            client = new TcpClient();
            Start();
       
        }

        /// <summary>
        /// starts the client activity - trying to connect to server, initialilzing componnents
        /// and waiting for events args
        /// </summary>
        public void Start()
        {

            Console.WriteLine("client: in start");
 
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000); //8000 = port
            try
            {
                client.Connect(ep);
                Console.WriteLine("client: client connected");
                this.Conected = true;

            }
            catch (Exception e)
            {
                this.Conected = false;
                Console.WriteLine("Error connecting to server:" + e.StackTrace);
            }

            try
            {
                this.stream = client.GetStream();
                this.reader = new BinaryReader(stream);
                this.writer = new BinaryWriter(stream);
                WaitForEventArgs();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error openning reader\\writer\\streamer :" + e.StackTrace);
            }
        }

        /// <summary>
        /// stops the client
        /// </summary>
        public void Stop()
        {
            client.Close();
        }


        /// <summary>
        /// sends a command to the server
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="args"></param>
        public void sendCommandRequest(int commandId, string[] args)
        {
            try
            {
                string path = null;
                if (commandId == (int)CommandEnum.CloseCommand)
                {
                    //this is the handler to remove 
                    path = args[0];
                }
                CommandRecievedEventArgs c = new CommandRecievedEventArgs(commandId, args, path);
                string cJson = JsonConvert.SerializeObject(c);
                writer.Write(cJson);
            }
            catch (Exception e)
            {
                Console.WriteLine("client: Error while sending command to server: " + e.StackTrace);
            }
        }

        /// <summary>
        /// wait for msgs from server in  new thread
        /// </summary>
        public void WaitForEventArgs()
        {
            {
        Task waitForEventsArgsFromServer = new Task (() =>
            {
                while (true)
                {
                    try
                    {
                     
                        this.stream = client.GetStream();
                        this.readLock.WaitOne();
                        string str = this.reader.ReadString();
                        this.readLock.ReleaseMutex();
                        Console.WriteLine("client: recieved msg from server: " + str);
                        MsgInfoEventArgs msgI = JsonConvert.DeserializeObject<MsgInfoEventArgs>(str);
                        DataRecieved?.Invoke(this, msgI);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("client: Error reading msg" + e.StackTrace);
                         
                    }
            }
            });
            waitForEventsArgsFromServer.Start();
            }
        }
    }
}
