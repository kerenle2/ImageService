using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ImageService;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            if (Environment.UserInteractive)
            {
                ImageService service1 = new ImageService(args);
                service1.TestStartupAndStop(args);
            }
            else
            {
                // Put the body of your old Main method here. 
                ServiceBase[] ServicesToRun;
                //LoggingModal loggingModal;
                ServicesToRun = new ServiceBase[]
                {
                new ImageService(args)
                };
                //loggingModal = new LoggingModal();
                ServiceBase.Run(ServicesToRun);
            }
         
        }
    }
}
