using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;


using System.Net;
using System.Net.Sockets;


namespace chatServer
{
    public class chatUser
    {

        private HttpListenerResponse resposne;

        public HttpListenerResponse HttpResponse
        {
            get { return resposne; }
            set { resposne = value; }
        }

        
    }
}
