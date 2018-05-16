using ImageService.Infrastructure.CommandsInfrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ImageService.Communication
{
    public class ClientHandler : IClientHandler
    {
        public ClientHandler()
        {

        }


        public void HandlerExecute()
        {

        }
        //public CommandRecievedEventArgs HandleClient(TcpClient client)
        //{
        //    Task handleClientRequest = new Task(() =>
        //    {
        //        using (NetworkStream stream = client.GetStream())
        //        using (BinaryReader reader = new BinaryReader(stream))
        //        using (BinaryReader writer = new BinaryReader(stream))
        //        {
        //            try
        //            {
        //                string commandLine = reader.ReadString();
        //                Console.WriteLine("Got command: {0}", commandLine);
        //                CommandRecievedEventArgs msgI = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        

        //             //   DataRecieved?.Invoke(this, msgI);

        //                //    string result = m_controller.ExecuteCommand(commandLine, client);
        //                string result;
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("server's clientHandler: Error reading line: " + e.StackTrace);
        //            }
        //            //handle the command here
                   
        //        }
        //        //client.Close();
        //    }); handleClientRequest.Start();
        //}


    }

   
}
