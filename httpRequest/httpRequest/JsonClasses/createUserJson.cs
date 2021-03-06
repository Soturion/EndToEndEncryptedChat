﻿using System;
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

    }
}
