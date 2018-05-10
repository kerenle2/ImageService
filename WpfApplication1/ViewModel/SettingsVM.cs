//using ImageService.ViewModel;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModel
{
    class SettingsVM : IViewModel
    {
        private SettingsModel model;
        public System.Windows.Input.ICommand RemoveCommand { get; private set; }

        // public ICommand RemoveCommand;

        public SettingsVM()
        {
        }

        public SettingsVM(SettingsModel model)
        {
            this.model = model;
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemoveClicked, this.CanRemove);

        }

        private void OnRemoveClicked(object obj)
        {
            //(can remove is called  here automatically)
            this.model.dirs.Remove(m_dirToRemove);
            //add here more needed instructions when removing
        }

        //this function called automatically when remove button is clicked
        private bool CanRemove(object obj)
        {
            bool resault = false;

            if (!string.IsNullOrEmpty(dirToRemove))
            {
                resault = true; //can remove
            }
            return resault; 
        }


        private string m_dirToRemove;
        public string dirToRemove
        {
            get { return this.m_dirToRemove; }
            set
            {
                m_dirToRemove = value;
              //  NotifyPropertyChanged("dirToRemove"); // needed here??
            }
        }



        #region getters
        public string outputDir
        {
            get { return this.model.outputDir; }

        }
        public string sourceName
        {
            get { return this.model.sourceName; }
        }

        public string thumbSize
        {
            get { return this.model.thumbSize; }
        }

        public string logName
        {
            get { return this.model.logName; }
        }

        public ObservableCollection<string> dirs
        {
            get { return this.model.dirs; }
        }
        #endregion



    }
}
