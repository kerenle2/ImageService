using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class MainWindowModel:IMainWindowModel
    {
        public bool Conected { get; set; }
        private Client client;
        public MainWindowModel()
        {
            this.client = Client.getInstance();
            this.Conected = this.client.Conected;
        }
    }
}
