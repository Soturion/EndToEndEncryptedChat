using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpRequest.JsonClasses
{
    class createUserJson
    {
        private httpServerCommands httpCommand = httpServerCommands.createUser;

        public httpServerCommands ServerCommand
        {
            get { return httpCommand; }
            set { httpCommand = value; }
        }

        private createUserData userToCreate;

        public createUserData NewUser
        {
            get { return userToCreate; }
            set { userToCreate = value; }
        }

        private string sSession = Form1.sSessionSeed;

        public string SessionSeed
        {
            get { return sSession; }
            set { sSession = value; }
        }


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
}
