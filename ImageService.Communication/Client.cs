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

        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;
        public bool Conected { get; set; }

        private NetworkStream stream = null;
        private static Client instance = null;

        public event EventHandler<EventArgs> DataRecieved;


        public static Client getInstance()
        {
            if (instance == null)
            {
                instance = new Client();
            }
            return instance;
        }




        private Client()
        {
            
            Console.WriteLine("client: in constructor");
            client = new TcpClient();
            Start();
       
        }

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

        public void Stop()
        {
            client.Close();
        }



        public void sendCommandRequest(int commandId, string[] args) //try with this format/ if no then change all the same to msgInfo (add more enums)
        {
            try
            {
                string path = null;
                if(commandId == (int)CommandEnum.CloseCommand)
                {
                    //this is the handler to remove ----HANDLE THIS, NOT YET HAVE IT...
                    path = args[0];
                }
                
                CommandRecievedEventArgs c = new CommandRecievedEventArgs(commandId, args, path);
                string cJson = JsonConvert.SerializeObject(c);
                writer.Write(cJson);
             //   Thread.Sleep(300);
            }
            catch (Exception e)
            {
                Console.WriteLine("client: Error while sending command to server: " + e.StackTrace);
            }
        }
        
        public void WaitForEventArgs()
        {
            {
        Task t = new Task (() =>
            {
                //loop here or outside task?
                while (true)
                {
                    try
                    {
                      //  Thread.Sleep(1000);

                        this.stream = client.GetStream();
                        string str = this.reader.ReadString();

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
            t.Start();
            }
        
        }


   

    }
}
