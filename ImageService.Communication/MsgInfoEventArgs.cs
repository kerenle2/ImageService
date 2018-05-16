using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class MsgInfoEventArgs : EventArgs
    {
        public MsgInfoEventArgs(MessagesToClientEnum id, string msg, TcpClient adrassedClient = null)
        {
            this.m_id = id;
            this.m_msg = msg;
        }

        private MessagesToClientEnum m_id;
        public MessagesToClientEnum id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_msg;
        public string msg
        {
            get { return m_msg; }
            set { m_msg = value; }
        }

        private TcpClient m_adrassedClient;
        public TcpClient adrassedClient
        {
            get { return m_adrassedClient; }
        }
    }
}
