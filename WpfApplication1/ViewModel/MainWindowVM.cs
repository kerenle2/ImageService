using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class MainWindowVM
    {
        private IMainWindowModel mainWindowModel;
        public bool ClientConnected { get { return mainWindowModel.Conected; } }
        public MainWindowVM()
        {
            this.mainWindowModel = new MainWindowModel();

        }
    }
}
