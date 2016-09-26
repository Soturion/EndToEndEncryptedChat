using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServer
{
    public class dbAction
    {
        public delegate void dbActionFinishedHandler(object sender, dbActionArgs e);

        public event dbActionFinishedHandler onDbActionFinished;

        private bool bHandeled = false;

        private string sCommand;

        private JsonClasses.createUserJson cUser;

        private int iStatus;

        private System.Net.HttpListenerContext context;

        private string sessionSeed;

        public string SessionSeed
        {
            get { return sessionSeed; }
            set { sessionSeed = value; }
        }


        public System.Net.HttpListenerContext HttpContext
        {
            get { return context; }
            set { context = value; }
        }

        public int Status
        {
            get { return iStatus; }
            set { iStatus = value; }
        }

        public JsonClasses.createUserJson ChatUser
        {
            get { return cUser; }
            set { cUser = value; }
        }

        private sqlAction dbAct;

        public sqlAction SqlAction
        {
            get { return dbAct; }
            set { dbAct = value; }
        }

        public string Command
        {
            get { return sCommand; }
            set { sCommand = value; }
        }

        public bool Handeled
        {
            get { return bHandeled; }
            set { bHandeled = value;
                if (bHandeled)
                {
                    dbActionArgs ret = new dbActionArgs(iStatus);
                    onDbActionFinished(this, ret);
                }
            }
        }
    }
}
