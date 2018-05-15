using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using ImageService.Infrastructure.CommandsInfrastructure;
using ImageService.Logging.Model;
using ImageService.Logging;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public ObservableCollection<MessageRecievedEventArgs> m_logMessage;
        private Client client;

        public event PropertyChangedEventHandler PropertyChanged;

        public LogModel()
        {
            this.client = Client.getInstance();
            //client.Start();
            this.m_logMessage = new ObservableCollection<MessageRecievedEventArgs>();
            this.client.DataRecieved += OnDataRecieved;
            System.Threading.Thread.Sleep(1000);
            //test!!!!
            MessageRecievedEventArgs e1 = new MessageRecievedEventArgs("Hey Efrat",MessageTypeEnum.INFO);
            MessageRecievedEventArgs e2 = new MessageRecievedEventArgs("Whats up???", MessageTypeEnum.INFO);
            MessageRecievedEventArgs e3 = new MessageRecievedEventArgs("Be careful!!!!", MessageTypeEnum.WARNING);
            MessageRecievedEventArgs e4 = new MessageRecievedEventArgs("ERRRRRRRRRROR", MessageTypeEnum.FAIL);

            logMessage.Add(e1);
            logMessage.Add(e2);
            logMessage.Add(e3);
            logMessage.Add(e4);

            //deleteeeeeee
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void OnDataRecieved(object sender, MsgInfoEventArgs e)
        {

            if (e.id == MessagesToClientEnum.Logs)
            {
                Console.WriteLine("I know i got an Logs msg!");
                List<Log> logsList = JsonConvert.DeserializeObject<List<Log>>(e.msg);
                string msg = e.msg;
                //do stuff here - handle the new logs list
                foreach (Log log in logsList)
                {
                    MessageRecievedEventArgs et = new MessageRecievedEventArgs(log.Message, log.Type); //deleteee
                    App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        logMessage.Add(et);
                    });
                }

                //MessageRecievedEventArgs et = new MessageRecievedEventArgs(e.msg, MessageTypeEnum.INFO); //deleteee
                //App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                //{
                //    logMessage.Add(et);
                //});
                
            }
        }

        public ObservableCollection<MessageRecievedEventArgs> logMessage
        {
            get { return this.m_logMessage; }
            set
            {
                m_logMessage = value;
                OnPropertyChanged("logMessage");
            }
        }

    }
}
