using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using System.Configuration;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Communication;
namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };


    public partial class ImageService: ServiceBase
    {

        //DELETE AFTER DEBUGGING

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        } // UNTIL HERE

        #region Members
        private ImageServer server;
        private IImageController controller;
        private IImageServiceModal modal;
        private ConfigData configData;
        public ILoggingService logger;
        #endregion
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="args"></param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            this.configData = ConfigData.InstanceConfig;


            //string m_OutputFolder = ConfigurationManager.AppSettings.Get("OuptputDir");
            //int m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            string m_OutputFolder = configData.OutputDir;
            int m_thumbnailSize = configData.ThumbnailSize;
            this.modal = new ImageServiceModal(m_OutputFolder, m_thumbnailSize);
            this.logger = new LoggingService();

            this.controller = new ImageController(this.modal, this.logger);

            string eventSourceName = this.configData.EventSourceName;
            string logName = this.configData.LogName;
           // string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            //string logName = ConfigurationManager.AppSettings.Get("LogName");
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }
        /// <summary>
        /// on start of the service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart");
            this.logger.Log("In On Start", MessageTypeEnum.INFO);
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //start server and logging modal:
            //this.logger = new LoggingService(); ///////////////////////changed
            logger.MessageRecieved += MessageReceivedLogger;
            this.server = new ImageServer(this.controller, this.logger);
            
        }
        /// <summary>
        /// write the message to the logger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void MessageReceivedLogger (object sender, MessageRecievedEventArgs message)
        {
            eventLog1.WriteEntry(message.Message);
            //add to loggerList

        }
        /// <summary>
        /// on stop of the service
        /// </summary>
        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
            this.server.Close();
            logger.MessageRecieved -= MessageReceivedLogger;
          
        }
 

    }
}
