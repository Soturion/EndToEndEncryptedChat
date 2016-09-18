using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Threading;
using BrightIdeasSoftware;


namespace chatPage
{
    public class serverCommunication
    {
        #region Private Deklarationen
        private int iCommunicationServerPort;
        private string sCommunicationAddress;
        private string sServerMessage = "";
        private chatMessage receivedChatMessage = new chatMessage();
        private List<chatMessage> msgList;
        private ObjectListView messageView;

        private NetworkStream communicationNetworkStream;
        private TcpClient communicationClient;
        private Thread tCommunicationThread;
        private byte[] bReceivedMessage = new byte[4096];
        byte[] bSendMessage = new byte[4096];
        #endregion

        #region Getter/Setter
        public Thread CommunicationThread
        {
            get { return tCommunicationThread; }
            set { tCommunicationThread = value; }
        }

        public int CommunicationServerPort
        {
            get { return iCommunicationServerPort; }
            set { iCommunicationServerPort = value; }
        }

        public string CommunicationServerAddress
        {
            get { return sCommunicationAddress; }
            set { sCommunicationAddress = value; }
        }

        public NetworkStream CommunicationNetworkStream
        {
            get { return communicationNetworkStream; }
            set { communicationNetworkStream = value; }
        }

        public TcpClient CommunicationClient
        {
            get { return communicationClient; }
            set { communicationClient = value; }
        }
        #endregion



        public enum serverCommands
        {
            nothing = 0,
            connect = 1,
            sendMessage = 2,
            disconnect = 3,
            getMessages = 4,

        }

        public serverCommunication(TcpClient client, List<chatMessage> lMessages, ObjectListView olv)
        {
            communicationClient = client;
            msgList = lMessages;
            messageView = olv;
            if (communicationClient != null)
            {
                communicationNetworkStream = client.GetStream();
                tCommunicationThread = new Thread(handleServer);
                tCommunicationThread.Start();
            }
        }

        private void handleServer()
        {
            while (communicationClient.Connected)
            {
                Array.Clear(bReceivedMessage, 0, bReceivedMessage.Length);
                try
                {
                    communicationNetworkStream.Read(bReceivedMessage, 0, bReceivedMessage.Length);
                }
                catch (System.IO.IOException ex)
                {
                    communicationClient.Close();
                }

                sServerMessage = Encoding.ASCII.GetString(bReceivedMessage, 0, bReceivedMessage.Length).Replace("\0", "");

                if (sServerMessage == "")
                {
                    communicationClient.Close();
                    communicationClient = null;
                    communicationNetworkStream.Close();
                    communicationNetworkStream = null;
                    return;
                }

                List<string> lSplit = sServerMessage.Split(new char[] { ';' }).ToList();

                switch ((serverCommands)Convert.ToInt32(lSplit[0]))
                {
                    case serverCommands.sendMessage:
                        receivedChatMessage = new chatMessage();
                        receivedChatMessage.User = lSplit[1];
                        receivedChatMessage.Message = lSplit[2];
                        msgList.Add(receivedChatMessage);
                        messageView.BuildList();
                        break;

                    case serverCommands.getMessages:
                        List<string> lOldMessages = sServerMessage.Remove(0,2).Split(new char[] { '|' }).ToList();

                        foreach(string s in lOldMessages)
                        {
                            if (s.Length < 1) { continue; }
                            lSplit = s.Split(new char[] { ';' }).ToList();
                            receivedChatMessage = new chatMessage();
                            receivedChatMessage.User = lSplit[0];
                            receivedChatMessage.Message = lSplit[3];
                            msgList.Add(receivedChatMessage);
                        }
                        messageView.BuildList();
                        break;

                }
            }
        }

        public int disconnect(string sRoom, string sPass, string sUser)
        {
            chatMessage cDisconnect = new chatMessage();
            cDisconnect.User = sUser;
            cDisconnect.Message = "dcMessage";
            //sendMessage(cDisconnect, serverCommands.disconnect);
            sendMessage("dcMessage", serverCommands.disconnect);
            //sendMessage(sRoom, sPass, sUser, cDisconnect, serverCommands.disconnect);
            return 0;
        }

        public int sendMessage(string messageText, serverCommands command)
        {
            if (communicationClient == null) { return -1; }
            if (!communicationClient.Connected) { return -2; }

            //chatMessage message = new chatMessage();
            //message.User = user;
            //message.Message = messageText;
            Array.Clear(bSendMessage, 0, bSendMessage.Length);

            bSendMessage = Encoding.ASCII.GetBytes((int)command + ";" + ";" + ";" + ";" + messageText);

            communicationNetworkStream.Write(bSendMessage, 0, bSendMessage.Length);
            return 0;
        }

        public int sendMessage(chatMessage message, serverCommands command)
        {
            if (communicationClient == null) { return -1; }
            if (!communicationClient.Connected) { return -2; }


            Array.Clear(bSendMessage, 0, bSendMessage.Length);

            bSendMessage = Encoding.ASCII.GetBytes((int)command + ";" + ";" + ";" + ";" + message.Message);

            communicationNetworkStream.Write(bSendMessage, 0, bSendMessage.Length);
            return 0;
        }

        public int connect(string sServer, string sPort, string sRoom, string sPass, string sUser)
        {
            Array.Clear(bSendMessage, 0, bSendMessage.Length);

            if (communicationClient == null)
            {
                communicationClient = new TcpClient();
            }

            if (communicationClient.Connected)
            {
                disconnect(sRoom, sPass, sUser);
                communicationClient.Close();
            }

            try
            {
                communicationClient.Connect(sServer, Convert.ToInt32(sPort));
                bSendMessage = Encoding.ASCII.GetBytes((int)serverCommands.connect + ";" + sRoom + ";" + sPass + ";" + sUser);
                if (communicationNetworkStream == null)
                {
                    communicationNetworkStream = communicationClient.GetStream();
                }
                communicationNetworkStream.Write(bSendMessage, 0, bSendMessage.Length);
            }
            catch (Exception ex)
            {
                return 1;
            }

            tCommunicationThread = new Thread(handleServer);
            tCommunicationThread.Start();
            return 0;

        }
    }
}
