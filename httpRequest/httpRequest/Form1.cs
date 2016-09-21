using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.WebSockets;
using System.IO;

using System.Web;
using System.Threading;

using Newtonsoft.Json;


namespace httpRequest
{
    public enum httpServerCommands
    {
        generateSessionSeed = 0,
        createUser = 1,
        loginUser = 2,
        sendMessage = 3,
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       public static string sSessionSeed = "";

        private async void button1_Click(object sender, EventArgs e)
        {
            //listBox1.Items.Add(DateTime.Now + ":" + DateTime.Now.Millisecond + " Send");
            JsonClasses.loginJson loginUser = new JsonClasses.loginJson();
            loginUser.UserData = new JsonClasses.createUserData();

            loginUser.UserData.UserName = tbUser.Text;
            loginUser.UserData.Password = tbPass.Text;
            loginUser.UserData.PublicKey = "linchen";

            string s = JsonConvert.SerializeObject(loginUser);

            sendCommand(s, "loginUser");

        }

        private async void sendCommand(string sJson, string sContentType)
        {
           

            if (sSessionSeed == "")
            {
                WebRequest generateSessionSeedRequest = WebRequest.Create("http://localhost:2222/");
                generateSessionSeedRequest.Method = "POST";

                string sEncryptedMessage = @"{ 'ServerCommand': '0' }";

                byte[] getSessionSeedBuffer = Encoding.UTF8.GetBytes(sEncryptedMessage);
                generateSessionSeedRequest.ContentType = "[serverCommand]createSessionSeed";
                generateSessionSeedRequest.ContentLength = getSessionSeedBuffer.Length;

                Stream sessionSeedReturn = generateSessionSeedRequest.GetRequestStream();
                sessionSeedReturn.Write(getSessionSeedBuffer, 0, getSessionSeedBuffer.Length);
                sessionSeedReturn.Close();

                sSessionSeed = ((JsonClasses.sessionSeedJson)JsonConvert.DeserializeObject<JsonClasses.sessionSeedJson>(await getResponseString(generateSessionSeedRequest))).SessionSeed;
                listBox1.Items.Add("Created session seed: " + sSessionSeed);
            }

            WebRequest requ = WebRequest.Create("http://localhost:2222/");
            requ.Method = "POST";

            byte[] sendServerCommand = Encoding.UTF8.GetBytes(sJson);
            requ.ContentType = "[serverCommand]" + sContentType;
            requ.ContentLength = sendServerCommand.Length;

            Stream dataStream = requ.GetRequestStream();
            dataStream.Write(sendServerCommand, 0, sendServerCommand.Length);
            dataStream.Close();

            switch(sContentType)
            {
                case "loginUser":
                    int iLoginResult = ((JsonClasses.actionReturnJson)JsonConvert.DeserializeObject<JsonClasses.actionReturnJson>(await getResponseString(requ))).Return;
                    break;
                case "createUser":
                    int iCreateResult = ((JsonClasses.actionReturnJson)JsonConvert.DeserializeObject<JsonClasses.actionReturnJson>(await getResponseString(requ))).Return;
                    break;
            }
            

        }

        private async Task<string> getResponseString(WebRequest request)
        {
            WebResponse test = await request.GetResponseAsync();
            byte[] bBuffer = new byte[1024];
            Stream stream = test.GetResponseStream();
            await stream.ReadAsync(bBuffer, 0, bBuffer.Length);
            stream.Close();

            if(Encoding.UTF8.GetString(bBuffer) == "")
            {
                return "-1";
            }

            return Encoding.UTF8.GetString(bBuffer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            JsonClasses.createUserJson createUser = new JsonClasses.createUserJson();
            createUser.NewUser = new JsonClasses.createUserData();

            createUser.NewUser.UserName = tbUser.Text;
            createUser.NewUser.Password = tbPass.Text;
            createUser.NewUser.PublicKey = "linchen";

            string s = JsonConvert.SerializeObject(createUser);

            sendCommand(s, "createUser");

        }
    }
}
