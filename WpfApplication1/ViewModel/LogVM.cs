using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class LogVM : INotifyPropertyChanged
    {
        private ILogModel logModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogVM()
        {
            this.logModel = new LogModel();

        }
        public LogVM(ILogModel model)
        {
            this.logModel = model;
        }
        public ObservableCollection<MessageRecievedEventArgs> logMessage
        {
            get { return this.logModel.logMessage; }
        }
    }
}
