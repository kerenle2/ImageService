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

        private static Client instance = null;

        public event EventHandler<MsgInfoEventArgs> DataRecieved;


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

            }
            catch (Exception e)
            {
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



        public void sendCommandRequest(int commandId) //try with this format/ if no then change all the same to msgInfo (add more enums)
        {
            try
            {
                //writeMutex.WaitOne();
                this.writer.Write(commandId.ToString());
                //  writeMutex.ReleaseMutex();
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
                       //  System.Threading.Thread.Sleep(200);
                        this.stream = client.GetStream();
                        //BinaryReader reader = new BinaryReader(stream);
                        //BinaryReader writer = new BinaryReader(stream);
                    

                        string str = this.reader.ReadString();
                       // System.Threading.Thread.Sleep(200);

                        Console.WriteLine("client: recieved msg from server: " + str);
                        //convert fron json 
                        //JObject messageObj = JObject.Parse(str);
                        //int id = (int)messageObj["TypeMessage"];

//                        string msg = (string)messageObj["Content"];
                        MsgInfoEventArgs msgI = JsonConvert.DeserializeObject<MsgInfoEventArgs>(str);
                        //MsgInfoEventArgs msgI = new MsgInfoEventArgs((MessagesToClientEnum)id, msg);
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
