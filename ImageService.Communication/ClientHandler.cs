using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



namespace ImageService.Communication
{
    public class ClientHandler : IClientHandler
    {
       
        public ClientHandler()
        {
           

        }

    

        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                      Console.WriteLine("Got command: {0}", commandLine);
                    //string result = m_controller.ExecuteCommand(commandLine, client);
                    //writer.Write(result);
                }
                client.Close();
            }).Start();
        }

   
        public void SendMsg(TcpClient client, MsgInfo msgI)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string msg = JsonConvert.SerializeObject(msgI);
                    writer.Write(msg);
                    //  Console.WriteLine("Got command: {0}", commandLine);
                    //string result = m_controller.ExecuteCommand(commandLine, client);
                    //writer.Write(result);
                }
                client.Close();
            }).Start();
        }
    }

   
}
