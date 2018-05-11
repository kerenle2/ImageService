using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class Client
    {

        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;
       
        private NetworkStream stream = null;
        private static Client instance = null;
        bool connected = false;

        //public event EventHandler<MsgInfo> RecivedLog;
        //public event EventHandler<string> RecivedSettings;
        // public event EventHandler<string> RecivedClose;   //needed?
        //public event EventHandler<string> RecivedHandlerRemoved;


        //public event EventHandler<MsgInfo> MsgRecieved;

       public TcpClient getMyTcpClient()
        {
            return this.client;
        }
            
        public void OnMsgRecieved(object sender, MsgInfo e)
        {
            Console.WriteLine("client: in onMsgRecieved function");
            if(e.id == MessagesToClientEnum.HandlerRemoved)
            {
                //do whatever
            }

            if(e.id == MessagesToClientEnum.Logs)
            {
                //do
            }

            if(e.id == MessagesToClientEnum.Settings)
            {
                //do
            }
        }


     

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
            start();
           // this.MsgRecieved += OnMsgRecieved;
        }

        public void start()
        {
            Console.WriteLine("client: in start");

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000); //or get int port in c'tor instead of 8000?
            try
            {
                client.Connect(ep);
                Console.WriteLine("client: client connected\n");
                this.connected = true;

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
              //  RecieveEventArgs();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error openning reader\\writer\\streamer :" + e.StackTrace);
            }



        }

        public void stop()
        {
            client.Close();
        }

       
        public MsgInfo RecieveEventArgs()
        {
            Task<MsgInfo> t = new Task<MsgInfo>(() =>
            {
                string msg = this.reader.ReadString();
                Console.WriteLine("client: recieved msg from server: " + msg);


               MsgInfo msgI =  JsonConvert.DeserializeObject<MsgInfo>(msg);
                this.OnMsgRecieved(this, msgI); //??

                  return msgI;
            });
            t.Start();
            return t.Result;

        }

        //not using this
        public string RecieveEventsMsgs()
        {
            Task<string> t = new Task<string>(()=>
            {
                string msg = this.reader.ReadString();
                Console.WriteLine("recieced msg from server: " + msg);
                return msg;
            });
            t.Start();
            return t.Result;

        }

  //      public string sendCommandRequestAndGetResponse(int commandEnum, ImageService.Infrastructure.Enums.CommandEnum e) //which of the 2 arguments?
   //     {
   //         Task<string> t = new Task<string>(()=>
   //         {
   //             sendMsg(commandEnum);
   //             string response = reciveMsg();
    //            return response;
     //           //th conversion of response to json will be in the client, so this class will stay unspecific. ?? or here?
      //      });
       //     t.Start();
        //    return t.Result;
       // }

    }
}
