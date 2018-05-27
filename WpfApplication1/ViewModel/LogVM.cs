using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageService.Logging.Model;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class LogVM : IViewModel
    {
        private ILogModel logModel;


        //constructor
        public LogVM()
        {
            this.logModel = new LogModel();
        }


        public ObservableCollection<MessageRecievedEventArgs> LogMessage
        {
            get { return this.logModel.logMessage; }
        }

        public ICommand CloseCommand { get; set; }

    }
}
