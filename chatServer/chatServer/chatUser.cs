using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace chatServer
{
    public class chatUser
    {
        //private room _roomOfUser;
        //private receivedCommand _receiveData;
        private TcpClient client;
        private NetworkStream netStream;
        private byte[] bSendMessage = new byte[4096];

        public TcpClient Client
        {
            get { return client; }
            set { client = value; }
        }

        //public receivedCommand ReceiveData
        //{
        //    get { return _receiveData; }
        //    set { _receiveData = value; }
        //}

        //public room RoomOfUser
        //{
        //    get { return _roomOfUser; }
        //    set { _roomOfUser = value; }
        //}

        //public chatUser(receivedCommand comm, room r, TcpClient tcp)
        //{
        //    _receiveData = comm;
        //    _roomOfUser = r;
        //    client = tcp;
        //}

        //public chatUser(receivedCommand comm, room r, TcpClient tcp)
        //{
        //    _receiveData = comm;
        //    _roomOfUser = r;
        //    client = tcp;
        //}

        public void sendMessage(string sMessage)
        {
            //if(netStream == null)
            //{
            //    netStream = client.GetStream();
            //}

            //Array.Clear(bSendMessage, 0, bSendMessage.Length);

            //bSendMessage = Encoding.ASCII.GetBytes((int)serverCommands.sendMessage + ";" + _receiveData.User + ";" + sMessage);
            //netStream.Write(bSendMessage, 0, bSendMessage.Length);
        }

        string sOldMsgTemp = "";

        public void sendOldMessages(string lmsg)
        {
            //if (netStream == null)
            //{
            //    netStream = client.GetStream();
            //}

            //Array.Clear(bSendMessage, 0, bSendMessage.Length);

            //bSendMessage = Encoding.ASCII.GetBytes(lmsg);
            //netStream.Write(bSendMessage, 0, bSendMessage.Length);

        }        
    }
}
