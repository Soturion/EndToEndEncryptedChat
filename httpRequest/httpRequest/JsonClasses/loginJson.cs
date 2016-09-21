using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpRequest.JsonClasses
{
    public class loginJson
    {
        private httpServerCommands serverCommand = httpServerCommands.loginUser;

        public httpServerCommands ServerCommand
        {
            get { return serverCommand; }
            set { serverCommand = value; }
        }

        private string sSession = Form1.sSessionSeed;

        public string SessionSeed
        {
            get { return sSession; }
            set { sSession = value; }
        }

        private createUserData loginUserData;

        public createUserData UserData
        {
            get { return loginUserData; }
            set { loginUserData = value; }
        }


    }
}
