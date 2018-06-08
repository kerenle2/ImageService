using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication;

namespace WebApplication2.Models
{
    public class CommunicationModel
    {
        protected Client client = null;
        public CommunicationModel()
        {
            this.client = Client.getInstance();
        }
    }
}
