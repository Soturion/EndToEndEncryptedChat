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
    }
}
