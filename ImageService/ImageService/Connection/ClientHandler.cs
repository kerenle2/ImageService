using ImageService.Controller;
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Connection
{
    class ClientHandler: IClientHandler
    {
        private IImageController m_controller;
        private ILoggingService m_logging;
        public ClientHandler(IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;

        }
        public void HandleClient(TcpClient client) {
            new Task(() => {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                    Console.WriteLine("Got command: {0}", commandLine);
                    string result = m_controller.ExecuteCommand((commandLine, client);
                    writer.Write(result); }
                client.Close();
            }).Start();
        }
    }
}
