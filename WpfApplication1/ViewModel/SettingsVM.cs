//using ImageService.ViewModel;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class SettingsVM : IViewModel
    {
        private SettingsModel model;

        public SettingsVM()
        {
        }

        public SettingsVM(SettingsModel model)
        {
            this.model = model;
        }

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



    }
}
