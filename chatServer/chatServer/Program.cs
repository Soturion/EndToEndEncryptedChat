using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Default Namespaces
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

//External Namespaces
using Newtonsoft.Json;

namespace chatServer
{
    class Program
    {
        private static HttpListener listener;
        static bool KeepGoing = true;

        static void Main(string[] args)
        {
            dbHelper.prepareDBConnection();
            dbHelper.lSqlCommands.CollectionChanged += lSqlCommands_CollectionChanged;

            listener = new HttpListener();
            listener.Prefixes.Add("http://+:2222/");
            listener.Start();

            
            ProcessAsync(listener);

            Thread tDBThread = new Thread(dbCommands);
            tDBThread.Start();

            new Thread(() => adminCommand()).Start();
        }

        static async Task ProcessAsync(HttpListener listener)
        {

            while (KeepGoing)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                new httpClientHandler(context);

            }
        }

        static void adminCommand()
        {
            string sAdminInput = "";
            while(((sAdminInput = Console.ReadLine()) != ""))
            {
                switch (sAdminInput.Trim())
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "list rooms":
                        Console.WriteLine(":: Rooms:");

                        //foreach(room r in RoomHandler.Rooms)
                        //{
                        //    Console.WriteLine(r.RoomName);
                        //}
                        break;

                    case "clear":
                        Console.Clear();
                        break;
                    case "remove room":
                        Console.WriteLine("Room to remove:");
                        //RoomHandler.removeRoom(Console.ReadLine());
                        break;
                }
            }
        }

        static void lSqlCommands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!dbHelper.bCleaningQueue)
                new Thread(() => dbCommands()).Start();
        }

        static string sGetMessagesCommand = "";
        private static void dbCommands()
        {
            dbAction loopedAct;
            for (int i = 0; i < dbHelper.lSqlCommands.Count; i++)
            {
                loopedAct = ((dbAction)dbHelper.lSqlCommands[i]);

                if (!loopedAct.Handeled & loopedAct.SqlAction == sqlAction.insert | loopedAct.SqlAction == sqlAction.createUser)
                {
                    dbHelper.executeQueryWithoutReturn(loopedAct);
                    dbHelper.bCleaningQueue = true;
                    dbHelper.lSqlCommands.RemoveAt(i);
                    dbHelper.bCleaningQueue = false;
                }
                else if (loopedAct.SqlAction == sqlAction.getMessages)
                {
                    if (loopedAct.ChatUser != null)
                    {
                        sGetMessagesCommand = dbHelper.executeGetMessages(loopedAct.Command);
                        loopedAct.ChatUser.sendOldMessages(sGetMessagesCommand);
                        dbHelper.bCleaningQueue = true;
                        dbHelper.lSqlCommands.RemoveAt(i);
                        dbHelper.bCleaningQueue = false;
                    }
                }
            }

        }
    }
}
