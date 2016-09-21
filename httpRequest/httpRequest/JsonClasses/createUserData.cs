using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpRequest.JsonClasses
{
    public class createUserData
    {
        private string sUsername;

        private string sPassword;

        private string sPubKey;

        public string PublicKey
        {
            get { return sPubKey; }
            set { sPubKey = value; }
        }

        public string Password
        {
            get { return sPassword; }
            set { sPassword = value; }
        }

        public string UserName
        {
            get { return sUsername; }
            set { sUsername = value; }
        }

    }
}
