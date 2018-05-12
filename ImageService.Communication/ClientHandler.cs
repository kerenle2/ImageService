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
                public event EventHandler<MsgInfoEventArgs> NotifyAllClients;

    
        public void HandlerExecute()
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
                    try
                    {
                        string commandLine = reader.ReadLine();
                        Console.WriteLine("Got command: {0}", commandLine);
                        // string result = m_controller.ExecuteCommand(commandLine, client);
                        string result;
                    } catch( Exception e)
                    {
                        Console.WriteLine("server's clientHandler: Error reading line: " + e.StackTrace);
                    }
             

                    //handle the command here
                    //with an event invoke, send msg back to all clients
                    
                    //writer.Write(result);
                }
                //client.Close();
            }).Start();
        }

 
    }

   
}
