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

namespace ImageServiceGUI.Model
{
    public class SettingsModel // : INotifyPropertyChanged  needed?!?
    {
        #region members
       private ICommunicate client;
       


        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion


        //constructor:
        public SettingsModel()
        {
            m_dirs = new ObservableCollection<string>();
            this.client = Client.getInstance();
            this.client.DataRecieved += OnDataRecieved;

            //client.Start();
            //delete:
            this.m_outputDir = "output";
            this.m_sourceName = "source";
            this.m_logName = "log";
            this.m_thumbSize = "size";
            this.dirs.Add("dir1");
            this.dirs.Add("dir2");
            //end delete

        }
        public void OnDataRecieved(object sender, EventArgs ee)
        {
            MsgInfoEventArgs e = (MsgInfoEventArgs)ee;
            if (e.id == MessagesToClientEnum.Settings)
            {
                Console.WriteLine("I know i got an settings msg!");
                //do stuff here - handle the app config list
            }
        }


        #region properties
        private string m_outputDir;
        public string outputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("outputDir"); //or other name?
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

        private ObservableCollection<string> m_dirs;
        public ObservableCollection<string> dirs
        {
            get { return m_dirs; }
            set
            {
                m_dirs = value;
                OnPropertyChanged("dirs");
            }
        }


        #endregion

    }
    #endregion
}
