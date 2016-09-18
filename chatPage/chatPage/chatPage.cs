using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using BrightIdeasSoftware;

namespace chatPage
{
    public partial class chatPage: TabPage
    {
        //Controls
        private Label lblServer;
        private Label lblRoom;
        private Label lblPass;
        private Label lblUser;

        private TextBox tbServer;
        private TextBox tbRoom;
        private TextBox tbPass;
        private TextBox tbUser;

        private Button btnConnect;
        private Button btnDisconnect;
        private Panel pnlBackground;

        private Button btnSend;

        private TextBox tbSend;

        public TextBox TextBoxSend
        {
            get { return tbSend; }
            set { tbSend = value; }
        }
        

        public Button ButtonSend
        {
            get { return btnSend; }
            set { btnSend = value; }
        }


        private ObjectListView olvMessages;

        public ObjectListView ObjectListViewMessages
        {
            get { return olvMessages; }
            set { olvMessages = value; }
        }

        private List<chatMessage> lChatMessages = new List<chatMessage>();

        public List<chatMessage> ChatMessages
        {
            get { return lChatMessages; }
            set { lChatMessages = value; }
        }

        private serverCommunication serverComm;

        public serverCommunication ServerCommunication
        {
            get { return serverComm; }
            set { serverComm = value; }
        }

        private TcpClient serverConnection;

        public TcpClient ServerConnection
        {
            get { return serverConnection; }
            set { serverConnection = value; }
        }

        public Button ButtonConnect
        {
            get { return btnConnect; }
            set { btnConnect = value; }
        }

        public chatPage(bool disableControls)
        {
            InitializeComponent();
            prepareControls();
            lblServer.Enabled = !disableControls;
            lblRoom.Enabled = !disableControls;
            tbServer.Enabled = !disableControls;
            tbRoom.Enabled = !disableControls;
            btnConnect.Enabled = !disableControls;
            pnlBackground.Enabled = !disableControls;

            serverComm = new serverCommunication(serverConnection, lChatMessages, olvMessages);
        }

        private void prepareControls()
        {
            pnlBackground = new Panel();
            pnlBackground.Dock = DockStyle.Fill;
            this.Controls.Add(pnlBackground);

            //Server Label
            if(lblServer == null)
            {
                lblServer = new Label();
            }

            lblServer.Text = "Server:";
            lblServer.Location = new Point(20, 20);
            lblServer.Width = 45;

            
            //Server Textbox
            if(tbServer == null)
            {
                tbServer = new TextBox();
            }
            tbServer.TextChanged += tbRoom_tbServer_TextChanged;
            tbServer.Location = new Point(lblServer.Location.X + lblServer.Width + 10, 18);
            tbServer.BringToFront();
            tbServer.Width = 150;
            tbServer.Text = "localhost:63383";

            if(lblRoom == null)
            {
                lblRoom = new Label();
            }

            //Raum Label
            lblRoom.Text = "Raum:";
            lblRoom.Location = new Point(lblServer.Location.X, lblServer.Location.Y + lblServer.Height + 5);
            lblRoom.Width = 40;

            if (tbRoom == null)
            {
                tbRoom = new TextBox();
            }

            //Raum TextBox
            tbRoom.TextChanged += tbRoom_tbServer_TextChanged;
            tbRoom.Location = new Point(lblRoom.Location.X + lblRoom.Width + 15, tbServer.Location.Y + tbServer.Height + 5);
            tbRoom.Width = 150;
            tbRoom.Text = "raum1";

            //Passwort Label
            if(lblPass == null)
            {
                lblPass = new Label();
            }

            lblPass.Location = new Point(lblRoom.Location.X, lblRoom.Location.Y + lblRoom.Height);
            lblPass.Text = "Passwort:";
            lblPass.Width = 55;

            //Passwort Textbox
            if (tbPass == null)
            {
                tbPass = new TextBox();
            }

            tbPass.Location = new Point(lblPass.Location.X + lblPass.Width, tbRoom.Location.Y + tbRoom.Height + 5);
            tbPass.Width = tbRoom.Width;
            tbPass.PasswordChar = '*';
            tbPass.Font = new Font(DefaultFont,FontStyle.Bold);
            tbPass.Text = "password";

            //User label
            if(lblUser == null)
            {
                lblUser = new Label();
            }

            lblUser.Location = new Point(lblRoom.Location.X, lblPass.Location.Y + lblPass.Height);
            lblUser.Text = "Name:";
            lblUser.Width = 55;

            //User Textbox

            if (tbUser == null)
            {
                tbUser = new TextBox();
            }

            tbUser.Location = new Point(lblUser.Location.X + lblUser.Width, tbPass.Location.Y + tbPass.Height + 5);
            tbUser.Width = tbRoom.Width;
            tbUser.Text = "UserName";

            //BtnConnect
            if(btnConnect == null)
            {
                btnConnect = new Button();
            }

            btnConnect.Text = "Verbinden";
            btnConnect.Location = new Point(lblUser.Location.X, lblUser.Location.Y + lblUser.Height);
            btnConnect.Click += btnConnect_Click;

            //Textverlauf
            olvMessages = new ObjectListView();
            OLVColumn colUser = new OLVColumn("Absender", "User");
            OLVColumn colSender = new OLVColumn("Nachricht", "Message");
            olvMessages.Columns.Add(colUser);
            olvMessages.Columns.Add(colSender);
            olvMessages.ColumnWidthChanged += olvMessages_ColumnWidthChanged;

            olvMessages.Location = new Point(btnConnect.Location.X, btnConnect.Location.Y + btnConnect.Width - 40);
            olvMessages.ShowGroups = false;
            olvMessages.Resize += olvMessages_Resize;
            olvMessages.SetObjects(lChatMessages);
            olvMessages.FullRowSelect = true;

            //Send Textbox
            if(tbSend == null)
            {
                tbSend = new TextBox();
            }

            tbSend.Multiline = false;
            tbSend.Height = 20;
            tbSend.KeyDown += tbSend_KeyDown;

            pnlBackground.Controls.Add(lblServer);
            pnlBackground.Controls.Add(tbServer);
            pnlBackground.Controls.Add(lblRoom);
            pnlBackground.Controls.Add(tbRoom);
            pnlBackground.Controls.Add(btnConnect);
            pnlBackground.Controls.Add(lblPass);
            pnlBackground.Controls.Add(lblUser);
            pnlBackground.Controls.Add(tbPass);
            pnlBackground.Controls.Add(tbUser);
            pnlBackground.Controls.Add(olvMessages);
            pnlBackground.Controls.Add(tbSend);
        }

        void olvMessages_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {

        }

        void tbSend_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 13)
            {
                //chatMessage message = new chatMessage();
                //message.User = tbUser.Text;
                //message.Message = tbSend.Text;
                //if (serverComm.sendMessage(message, serverCommunication.serverCommands.sendMessage) != 0)
                if(serverComm.sendMessage(tbSend.Text,serverCommunication.serverCommands.sendMessage) != 0)
                {
                    MessageBox.Show("Nicht verbunden", "Chatclient", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                tbSend.Text = "";
                e.Handled = true;
            }
            
        }

        void olvMessages_Resize(object sender, EventArgs e)
        {
            if (tbSend == null) { return; }
            tbSend.Location = new Point(olvMessages.Location.X, olvMessages.Location.Y + olvMessages.Height + 20);
            tbSend.Width = olvMessages.Width;
            olvMessages.Columns[1].Width = olvMessages.Width - olvMessages.Columns[0].Width - 10;
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            serverComm.connect(tbServer.Text.Substring(0, tbServer.Text.IndexOf(":")), tbServer.Text.Substring(tbServer.Text.IndexOf(":") + 1, tbServer.Text.Length - tbServer.Text.IndexOf(":") - 1), tbRoom.Text, tbPass.Text, tbUser.Text);
        }

        public void disconnect()
        {
            serverComm.disconnect(tbRoom.Text, tbPass.Text, tbUser.Text);
        }

        string sServerText;
        string sRoomText;

        void tbRoom_tbServer_TextChanged(object sender, EventArgs e)
        {
            sServerText = "";
            sRoomText = "";

            if(tbServer != null)
            {
                sServerText = tbServer.Text;
            }

            if(tbRoom != null)
            {
                sRoomText = tbRoom.Text;
            }

            this.Text = sServerText + " - " + sRoomText;
        }
    }
}
