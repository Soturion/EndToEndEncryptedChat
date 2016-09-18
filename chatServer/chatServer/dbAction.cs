using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServer
{
    public class dbAction
    {
        private bool bHandeled = false;

        private string sCommand;

        private chatUser cUser;

        public chatUser ChatUser
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
            set { bHandeled = value; }
        }

    }
}
