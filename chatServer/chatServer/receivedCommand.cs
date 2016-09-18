using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace chatServer
{
    //public class receivedCommand
    //{
    //    private List<string> lMessageSplit = new List<string>();
    //    private string sCommandRoom = "";
    //    private string sCommandPass = "";
    //    private string sCommandUser = "";
    //    private string sCommandData = "";
    //    private serverCommands commandType = serverCommands.nothing;

    //    public serverCommands Action
    //    {
    //        get { return commandType; }
    //        set { commandType = value; }
    //    }

    //    public string Room
    //    {
    //        get { return sCommandRoom; }
    //        set { sCommandRoom = value; }
    //    }

    //    public string Pass
    //    {
    //        get { return sCommandPass; }
    //        set { sCommandPass = value; }
    //    }

    //    public string User
    //    {
    //        get { return sCommandUser; }
    //        set { sCommandUser = value; }
    //    }

    //    public string Data
    //    {
    //        get { return sCommandData; }
    //        set { sCommandData = value; }
    //    }

    //    public void getData(string message)
    //    {
    //        lMessageSplit = new List<string>();
    //        lMessageSplit = message.Split(new char[] { ';' }).ToList();
    //        int iCounter = 0;
    //        foreach(PropertyInfo pInfo in this.GetType().GetProperties())
    //        {
    //            if(iCounter == 0)
    //            {
    //                pInfo.SetValue(this, (serverCommands)Convert.ToInt32(lMessageSplit[iCounter]),null);
    //            }
    //            else
    //            {
    //                if (iCounter >= lMessageSplit.Count) { continue; }
    //                if (pInfo.GetValue(this, null).ToString().Length > 0 & lMessageSplit[iCounter].Length < 1) 
    //                {
    //                    iCounter++;
    //                    continue; 
    //                }
    //                pInfo.SetValue(this, lMessageSplit[iCounter], null);
    //            }
    //            iCounter++;
    //        }
    //    }

    //    public string getLogMessage()
    //    {
    //        string sLogMessage = "";
    //        foreach(PropertyInfo pInfo in this.GetType().GetProperties())
    //        {
    //            sLogMessage += pInfo.Name + ":" + pInfo.GetValue(this, null).ToString() + " ";
    //        }
    //        return sLogMessage;
    //    }

    //    public room getRoomFromCommand()
    //    {
    //        if(!RoomHandler.checkRoomExist(sCommandRoom))
    //        {
    //            room r = new room();
    //            r.RoomName = sCommandRoom;
    //            r.RoomPassword = sCommandPass;
    //            return r;
    //        }
    //        return null;
    //    }

    //}
}
