using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class Client
    { 
        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;
        private NetworkStream stream = null;
        private static Client instance = null;
         
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
            this.client = new TcpClient();
        }

        public void start()
        {
        
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000); //or get int port in c'tor instead of 8000?
            try
            {
                client.Connect(ep);
                Console.WriteLine("client connected\n");
            } catch(Exception e)
            {
                Console.WriteLine("Error connecting to server:" + e.StackTrace);
            }

            try
            {
                this.stream = client.GetStream();
                this.reader = new BinaryReader(stream);
                this.writer = new BinaryWriter(stream);
            } catch(Exception e)
            {
                Console.WriteLine("Error openning reader\\writer\\streamer :" + e.StackTrace);
            }
          


        }

        public void stop()
        {
            client.Close();
        }

        public void sendMsg()
        {
       
        }

        public void reciveMsg()
        {

        }
    }
   
    
}
    
