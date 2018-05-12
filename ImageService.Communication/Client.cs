﻿using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class Client : ICommunicate
    {

        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;

       
        private NetworkStream stream = null;
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



        public void OnDataRecieved(object sender, MsgInfoEventArgs e)
        {
            Console.WriteLine("client: in onMsgRecieved function");
            if(e.id == MessagesToClientEnum.HandlerRemoved)
            {
                //do whatever
                Console.WriteLine("I know i got an handlerRemoved msg!");
            }

            if(e.id == MessagesToClientEnum.Logs)
            {
                //do
                Console.WriteLine("I know i got an Logs msg!");

            }

            if (e.id == MessagesToClientEnum.Settings)
            {
                //do
                Console.WriteLine("I know i got an settings msg!");

            }
        }


        private Client()
        {
            Console.WriteLine("client: in constructor");
            client = new TcpClient();
       
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
                this.writer.Write(commandId.ToString());
            } catch (Exception e)
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
                    string msg = this.reader.ReadString();
                Console.WriteLine("client: recieved msg from server: " + msg);
                MsgInfoEventArgs msgI = JsonConvert.DeserializeObject<MsgInfoEventArgs>(msg);
                DataRecieved?.Invoke(this, msgI);
            }

            });
            t.Start();
            }
        

        }




    }
}
