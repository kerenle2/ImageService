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
    class LogVM
    {
        private ILogModel logModel;
        private ICommand guiDisconnected;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogVM()
        {
            this.logModel = new LogModel();
           // this.guiDisconnected = new DelegateCommand<object>(this.OnDisconnectCommand, this.CanDisconnect);

        }
        public LogVM(ILogModel model)
        {
            this.logModel = model;
           // this.guiDisconnected = new DelegateCommand<object>(this.OnDisconnectCommand, this.CanDisconnect);

        }
        public ObservableCollection<MessageRecievedEventArgs> LogMessage
        {
            get { return this.logModel.logMessage; }
        }
       // public bool ClientConnected { get { return logModel.Conected; } }
        public ICommand CloseCommand { get; set; }
        //private void OnDisconnectCommand(object obj)
        //{
        //    Console.Write("lsldfkdsjldsfj");
        //}
        //private bool CanDisconnect(object obj)
        //{
        //    return true;
        //}



    }
}
