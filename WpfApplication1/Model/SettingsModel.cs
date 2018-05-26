using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ImageServiceGUI.Model
{
    public class SettingsModel // : INotifyPropertyChanged  //needed?!?
    {
        #region members
       private Client client;
        //  private ICommunicate client;
        private Mutex removeHandlerLock = new Mutex();
        private Mutex configLock = new Mutex();
        private ObservableCollection<string> m_dirs;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }



        //constructor:
        public SettingsModel()
        {
           // System.Threading.Thread.Sleep(1000);

            //
            m_dirs = new ObservableCollection<string>();
            this.client = Client.getInstance();
            this.client.DataRecieved += OnDataRecieved;


        }
        public void OnDataRecieved(object sender, EventArgs ee)
        {
            MsgInfoEventArgs e = (MsgInfoEventArgs)ee;
            if (e.id == MessagesToClientEnum.Settings)
            {
               // this.configLock.WaitOne();
                Console.WriteLine("I know i got an settings msg!");
                FromJson(e.msg);
             //  this.configLock.ReleaseMutex();

            }
            if(e.id == MessagesToClientEnum.HandlerRemoved)
            {
                //removeHandlerLock.WaitOne();
                App.Current.Dispatcher.Invoke((Action)delegate // <--- here
                {
                    this.dirs.Remove(e.msg);
                });
               // removeHandlerLock.ReleaseMutex();
            }
        }

        public void FromJson(string str)
        {
            JObject configJson = JObject.Parse(str);
            List<string> handlers = (configJson["Handlers"]).ToObject<List<string>>();
            foreach (string handler in handlers)
            {
               // this.configLock.WaitOne();

                App.Current.Dispatcher.Invoke((Action)delegate // <--- here
                {

                    this.dirs.Add(handler);
                });
                //this.configLock.ReleaseMutex();
            }
            string LogName = configJson["LogName"].ToObject<string>();
            logName = LogName;
            this.sourceName = (string)configJson["EventSourceName"];
            this.outputDir = (string)configJson["OutputDir"];
            this.thumbSize = ((int)configJson["ThumbnailSize"]).ToString(); //check here to string.


        }

        private string m_outputDir;
        public string outputDir
        {
            get { return m_outputDir; }
            set
            {
                
                    m_outputDir = value;
                    OnPropertyChanged("outputDir");
                
            }
        }

        private string m_sourceName;
        public string sourceName
        {
            get { return m_sourceName; }
            set
            {
                m_sourceName = value;
                OnPropertyChanged("sourceName");
            }
        }

        private string m_dirToRemove;
        public string dirToRemove
        {
           get { return this.m_dirToRemove; }
            set
            {
                m_dirToRemove = value;
                OnPropertyChanged("dirToRemove");

            }
        }
        private string m_logName;
        public string logName
        {
            get { return m_logName; }
            set
            {
                m_logName = value;
                OnPropertyChanged("logName");
            }
        }

        private string m_thumbSize;
        public string thumbSize
        {
            get { return m_thumbSize; }
            set
            {
                m_thumbSize =  value;
                OnPropertyChanged("thumbSize");
            }
        }

       
        public ObservableCollection<string> dirs
        {
            get { return m_dirs; }
            set
            {
                m_dirs = value;
                OnPropertyChanged("dirs");
            }
        }
        public void RemoveHandler(String dir)
        {
            string[] args = { dir } ;
            client.sendCommandRequest((int)CommandEnum.CloseCommand, args);

        }
    }
}
