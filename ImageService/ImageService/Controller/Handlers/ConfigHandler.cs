using ImageService.Communication;
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
    class ConfigHandler: IHandler
    {
        //members
        public IImageController controller;
        ConfigData data = ConfigData.InstanceConfig;

        //constructor
        public ConfigHandler(IImageController m_controller)
        {
            this.controller = m_controller;
            controller.RequestData += OnRequestData;
        }

        /// <summary>
        /// sends the app config in a new thread.
        /// </summary>
        /// <param name="e"></param>
        void handleSendConfigRequst(RequestDataEventArgs e)
        {
            Task sendAppConfig = new Task(() =>
            {
                string JsonList = data.ToJSON();
                MsgInfoEventArgs msgI = new MsgInfoEventArgs(MessagesToClientEnum.Settings, JsonList);
                controller.SendToServer(msgI, e.client);
            });
            sendAppConfig.Start();
        }

        /// <summary>
        /// activated when controller asks for data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRequestData(object sender, RequestDataEventArgs e)
        {
            handleSendConfigRequst(e);
        }
    }
}
