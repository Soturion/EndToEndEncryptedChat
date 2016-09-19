using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServer
{
    public class dbActionArgs : EventArgs
    {
        private int returnCode;

        public int ReturnCode
        {
            get
            {
                return returnCode;
            }            
        }

        public dbActionArgs(int returnCode)
        {
            this.returnCode = returnCode;
        }
    }
}
