using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class SettingsVM 
    {
        private SettingsModel model = new SettingsModel();

        public SettingsVM()
        {
        }

        public SettingsVM(SettingsModel model)
        {
            this.model = model;
        }
        
    }
}
