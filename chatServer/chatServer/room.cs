using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace chatServer
{
    public class room
    {
        private string sName;
        private string sPassword;
        private List<chatUser> lClients = new List<chatUser>();

        private DateTime dtCreation = DateTime.Now;

        public DateTime CreationDate
        {
            get { return dtCreation; }
            set { dtCreation = value; }
        }

        public string RoomPassword
        {
            get { return sPassword; }
            set { sPassword = value; }
        }
        
        public string RoomName
        {
            get { return sName; }
            set { sName = value; }
        }
        
        public List<chatUser> RoomClients
        {
            get { return lClients; }
            set { lClients = value; }
        }

        public void broadcastMessage(string sMessage)
        {
            foreach (chatUser cuser in lClients)
            {
                cuser.sendMessage(sMessage);
            }
        }

    }
}
