using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ImageServiceGUI.Model
{
    public class SettingsModel : INotifyPropertyChanged
    {
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

            //delete:
            this.outputDir = "output";
            this.sourceName = "source";
            this.logName = "log";
            this.thumbSize = "size";
            //end delete

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
}
