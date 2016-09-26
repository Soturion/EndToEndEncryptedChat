using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServer
{
    public static class sessionHandler
    {
        public static Dictionary<string, chatUser> sessions = new Dictionary<string, string>();
    }
}
