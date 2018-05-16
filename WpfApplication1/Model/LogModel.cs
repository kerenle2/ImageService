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
using System.Threading;
using ImageService.Logging;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public ObservableCollection<MessageRecievedEventArgs> m_logMessage;
        private Client client;
        private Mutex listLock = new Mutex();
        public event PropertyChangedEventHandler PropertyChanged;

        public LogModel()
        {
            this.client = Client.getInstance();
            this.m_logMessage = new ObservableCollection<MessageRecievedEventArgs>();
            this.client.DataRecieved += OnDataRecieved;

            System.Threading.Thread.Sleep(1000);
    //           client.sendCommandRequest((int)CommandEnum.LogCommand, null);       //check that here is the spot.  MORE / LESS SLEEP?? 


        }
        protected void OnPropertyChanged(string name)
        {
               if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void OnDataRecieved(object sender, EventArgs ee)
        {
            MsgInfoEventArgs e = (MsgInfoEventArgs)ee;
            if (e.id == MessagesToClientEnum.Logs)
            {
                Console.WriteLine("I know i got an Logs msg!");
                List<Log> logsList = JsonConvert.DeserializeObject<List<Log>>(e.msg);
                // string msg = e.msg;
                //do stuff here - handle the new logs list
                listLock.WaitOne();
                foreach (Log log in logsList)
                {
                    MessageRecievedEventArgs et = new MessageRecievedEventArgs(log.Message, log.Type);
                    App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        
                        logMessage.Add(et);
                    });
                }
                listLock.ReleaseMutex();


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
