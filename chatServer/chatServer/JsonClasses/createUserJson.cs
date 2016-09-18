using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServer.JsonClasses
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

        public class createUserData
        {
            private string sUsername;

            private string sPassword;

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

            private string sPubKey;

            public string PublicKey
            {
                get { return sPubKey; }
                set { sPubKey = value; }
            }

        }
    }
}
