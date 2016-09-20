using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Npgsql;
namespace chatServer
{
    public enum sqlAction
    {
        insert = 0,
        getMessages = 1,
        deleteMessages = 2,
        createUser = 3,
    }

    public static class dbHelper
    {
        public static void prepareDBConnection()
        {
            dataBaseHelper.getDbData();
            NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = dataBaseHelper.sDbAddress;
            stringBuilder.Port = dataBaseHelper.iDbPort;
            stringBuilder.Username = dataBaseHelper.sDbUser;
            stringBuilder.Password = dataBaseHelper.sDbPass;
            stringBuilder.Database = "chat_db";
            
            dbHelper.connection = new NpgsqlConnection(stringBuilder);
            dbHelper.connection.Open();

            Console.WriteLine("DB connection established");
            
            prepareTables();
        }

        public static NpgsqlConnection connection;

        private static NpgsqlCommand createChatTab;
        private static NpgsqlCommand createUserTab;

        public static bool bCleaningQueue = false;
        public static ObservableCollection<dbAction> lSqlCommands = new ObservableCollection<dbAction>();

        private static void prepareTables()
        {
            createChatTab = new NpgsqlCommand(@"CREATE TABLE chat_tab (uname text, room text, counter SERIAL NOT NULL, message text, PRIMARY KEY(uname, room, counter));", dbHelper.connection);
            createUserTab = new NpgsqlCommand(@"CREATE TABLE user_tab (uname text, publickey text, credat date, PRIMARY KEY(uname));", dbHelper.connection);

            try
            {
                createChatTab.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"-- Prepare Table Exception: " + ex.Message);
            }

            try
            {
                createUserTab.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"-- Prepare Table Exception: " + ex.Message);
            }
        }

        public static void queueInsertMessage(string sUser, string sRoom, string sMessage)
        {
            dbAction act = new dbAction();
            act.Command = @"INSERT INTO chat_tab (uname, room, message) VALUES('[uname]','[room]','[message]');".Replace("[uname]", sUser).Replace("[room]",sRoom).Replace("[message]", sMessage);
            act.SqlAction = sqlAction.insert;
            dbHelper.lSqlCommands.Add(act);
        }

        private static List<string> lMessages = new List<string>();

        public static void queueDeleteMessages(string sRoom)
        {
            dbAction act = new dbAction();
            act.Command = @"DELETE FROM chat_tab where room = [room]".Replace("[room]", "'" + sRoom + "'");
            act.SqlAction = sqlAction.deleteMessages;
            dbHelper.lSqlCommands.Add(act);
        }

        public static dbAction queueCreateUser(string sUserName, string sPublicKey, DateTime credat)
        {
            dbAction act = new dbAction();
            act.Command = @"INSERT INTO user_tab (uname, publickey, credat) VALUES('[uname]','[publickey]','[credat]');".Replace("[uname]", sUserName).Replace("[publickey]", sPublicKey).Replace("[credat]", credat.ToShortDateString());
            act.SqlAction = sqlAction.createUser;
            dbHelper.lSqlCommands.Add(act);
            return act;
        }

        public static void executeQueryWithoutReturn(dbAction action)
        {
            if (createChatTab != null & connection != null)
            {
                createChatTab.CommandText = action.Command;

                try
                {
                    int result =  createChatTab.ExecuteNonQuery();
                    if(result > 0)
                    {
                        action.Status = result;
                    }

                    action.Handeled = true;

                }
                catch (Exception ex)
                {
                    action.Status = -1;
                    action.Handeled = true;
                    Console.WriteLine("++ Query queue error: " + ex.Message);
                }
                
            }

            action.Status = -3;
        }

        private static void Action_onDbActionFinished(object sender, dbActionArgs e)
        {
            throw new NotImplementedException();
        }

        private static NpgsqlDataReader getMessagesReader;
        public static string executeGetMessages(string sQuery)
        {
            return "";
            //if(command != null & connection != null)
            //{
            //    command.CommandText = sQuery;
            //    try
            //    {
            //        getMessagesReader = command.ExecuteReader();
            //        string sResult = Convert.ToInt32(serverCommands.getMessages).ToString() + ";";
            //        object[] colResult = new object[4];
            //        while(getMessagesReader.Read())
            //        {
            //            getMessagesReader.GetValues(colResult);
            //            foreach(object s in colResult)
            //            {
            //                sResult += s + ";";
            //            }
            //            sResult += "|";
            //        }
            //        getMessagesReader.Close();
                    
            //        return sResult;
            //    }
            //    catch (Exception ex)
            //    {

            //        return "";
            //    }
            //}
            //return "";

        }

    }
}
