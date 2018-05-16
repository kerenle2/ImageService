using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    class ConfigHandler: IConfigHandler
    {
        public IImageController controller;
        public ConfigHandler(IImageController m_controller)
        {
            this.controller = m_controller;

        }

        void sendMessage(CommandRecievedEventArgs e)
        {
            Task sendAppConfig = new Task(() =>
            {
                bool result;
                string msg = controller.ExecuteCommand(e.CommandID, e.Args, out result);
                //if (result)
                //{
                //    this.m_logging.Log(msg, MessageTypeEnum.INFO);
                //}
                //else
                //{
                //    this.m_logging.Log(msg, MessageTypeEnum.FAIL);
                //}
            });
            sendAppConfig.Start();
            //convert to jason and send to server
        }
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                sendMessage(e); //// if its the command, send back to server the list. (but with jason)
            }
        }
    }
}
