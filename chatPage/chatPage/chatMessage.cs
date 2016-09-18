using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chatPage
{
    public class chatMessage
    {
        private string sUser;
        private string sMessage;

        public string Message
        {
            get { return sMessage; }
            set { sMessage = value; }
        }

        public string User
        {
            get { return sUser; }
            set { sUser = value; }
        }

    }
}
