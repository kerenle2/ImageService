using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class Client : ICommunicate
    {

        private static TcpClient client;


        private Mutex writeMutex;
        private Mutex readMutex;

        private NetworkStream stream = null;
        private StreamReader reader;
        private StreamWriter writer;

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
            writeMutex = new Mutex();
            readMutex = new Mutex();
          //  Start();

        }

        public void Start()
        {

            Console.WriteLine("client: in start");
 
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000); //8000 = port
            try
            {
                client.Connect(ep);
                Console.WriteLine("client: client connected");

                stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error connecting to server:" + e.StackTrace);
            }

            try
            {
                Task waitToEvents = new Task(() =>

                {
                    WaitForEventArgs();

                });
                waitToEvents.Start();
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
                while (true)
                {
                    try
                    {

                    //  readMutex.WaitOne();
                    stream = client.GetStream();
                    reader = new StreamReader(stream);

                    string msg = this.reader.ReadLine();
                        //   readMutex.ReleaseMutex();
                        //Console.WriteLine("client: recieved msg from server: " + msg);

                        MsgInfoEventArgs msgI = JsonConvert.DeserializeObject<MsgInfoEventArgs>(msg);
                        DataRecieved?.Invoke(this, msgI);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("client: Error reading msg" + e.StackTrace);
                    }
                }
        
        }
    }
    }


   

