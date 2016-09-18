using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace chatServer
{
    public static class dataBaseHelper
    {
        #region Deklarationen
        public static string sDbUser;
        public static string sDbPass;
        public static int iDbPort;
        public static string sDbAddress;
        public static XmlDocument xmlDoc = new XmlDocument();
        #endregion

        public static void getDbData()
        {
            if(xmlDoc == null)
            xmlDoc = new XmlDocument();

            xmlDoc.RemoveAll();
            xmlDoc.Load("dbConfig.xml");

            foreach(XmlNode configNode in xmlDoc.DocumentElement.ChildNodes)
            {
                switch(configNode.Attributes["name"].Value)
                {
                    case "address":
                        sDbAddress = configNode.Attributes["value"].Value;
                        break;
                    case "port":
                        iDbPort = Convert.ToInt32(configNode.Attributes["value"].Value);
                        break;
                    case "user":
                        sDbUser = configNode.Attributes["value"].Value;
                        break;
                    case "pw":
                        sDbPass = configNode.Attributes["value"].Value;
                        break;
                }
            }
        }
    }
}
