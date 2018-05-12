using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ImageService.Communication;

namespace ImageServiceGUI.Model
{
    interface ILogModel
    {
        ObservableCollection<MessageRecievedEventArgs> logMessage { get; set; }
        void OnDataRecieved(object sender, MsgInfoEventArgs e);

    }
}
