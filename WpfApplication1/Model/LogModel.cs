using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ImageService.Communication;

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
            this.m_logMessage = new ObservableCollection<MessageRecievedEventArgs>();
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
