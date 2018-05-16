using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ImageService.Communication;
using ImageService.Logging.Model;

namespace ImageServiceGUI.Model
{
    interface ILogModel
    {
        ObservableCollection<MessageRecievedEventArgs> logMessage { get; set; }
        void OnDataRecieved(object sender, EventArgs e);

    }
}
