using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace chatServer
{
    public static class RoomHandler
    {

        private static List<room> lRooms = new List<room>();

        public static List<room> Rooms
        {
            get { return lRooms; }
            set { lRooms = value; }
        }

        public static void removeRoom(string sRoomName)
        {
            room rToRemove = findRoom(sRoomName);
            if(rToRemove != null)
            {
                int iC = rToRemove.RoomClients.Count;
                for(int i = 0; i < iC; ++i)
                {
                    RoomHandler.disconnectUser(((chatUser)rToRemove.RoomClients[i]), false);
                    ((chatUser)rToRemove.RoomClients[i]).RoomOfUser = null;
                }

                Rooms.Remove(rToRemove);
                rToRemove = null;
                dbHelper.queueDeleteMessages(sRoomName);
                Console.WriteLine(">> Removed room: " + sRoomName);
            }
            else
            {
                Console.WriteLine(">> Room not found: " + sRoomName);
            }
        }

        public static bool checkRoomExist(string searchName)
        {
            foreach(room r in lRooms)
            {
                if(r.RoomName == searchName)
                {
                    return true;
                }
            }

            return false;
        }

        public static room findRoom(string searchName)
        {
            foreach(room r in lRooms)
            {
                if(r.RoomName == searchName)
                {
                    return r;
                }
            }

            return null;
        }

        private static bool checkIfUserExistsInRoom(room r, chatUser user)
        {
            foreach(chatUser loopUser in r.RoomClients)
            {
                if(loopUser == user)
                {
                    return true;
                }
            }

            return false;
        }

        public static int disconnectUser(chatUser user, bool bRemoveUserFromList = false)
        { 
            //if(user.RoomOfUser != null)
            //{
            //    if(checkIfUserExistsInRoom(user.RoomOfUser, user))
            //    {
            //        user.Client.Close();

            //        if(bRemoveUserFromList)
            //        {
            //            user.RoomOfUser.RoomClients.Remove(user);
            //        }
                    
            //        Console.WriteLine(">> Disconnected: " + user.ReceiveData.User + " from Room: " + user.RoomOfUser.RoomName);
            //    }

            //    DateTime dtCalc = user.RoomOfUser.CreationDate.AddDays(7);

            //    if (user.RoomOfUser.RoomClients.Count < 1 & user.RoomOfUser.CreationDate > dtCalc)
            //    {
            //        Console.WriteLine(">> Removed Room:" + user.RoomOfUser.RoomName);

            //        dbHelper.queueDeleteMessages(user.RoomOfUser.RoomName);
            //        RoomHandler.Rooms.Remove(user.RoomOfUser);   
            //    }
            //}
            return 0;
        }
    }
}
