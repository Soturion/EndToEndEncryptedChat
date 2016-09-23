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
        loginUser = 4,
    }

    public static class dbHelper
    {
        public static void prepareDBConnection()
        {
            dataBaseXmlHelper.getDbData();
            NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = dataBaseXmlHelper.sDbAddress;
            stringBuilder.Port = dataBaseXmlHelper.iDbPort;
            stringBuilder.Username = dataBaseXmlHelper.sDbUser;
            stringBuilder.Password = dataBaseXmlHelper.sDbPass;
            stringBuilder.Database = "chat_db";

            dbHelper.connection = new NpgsqlConnection(stringBuilder);
            dbHelper.connection.Open();

            Console.WriteLine("DB connection established");

            prepareTables();
        }

        public static NpgsqlConnection connection;

        private static NpgsqlCommand dbCommand;
        private static NpgsqlCommand createUserTab;

        public static bool bCleaningQueue = false;
        public static ObservableCollection<dbAction> lSqlCommands = new ObservableCollection<dbAction>();

        private static void prepareTables()
        {
            dbCommand = new NpgsqlCommand(@"CREATE TABLE chat_tab (uname text, room text, counter SERIAL NOT NULL, message text, PRIMARY KEY(uname, room, counter));", dbHelper.connection);
            createUserTab = new NpgsqlCommand(@"CREATE TABLE user_tab (uname text, password text, publickey text, credat date, PRIMARY KEY(uname));", dbHelper.connection);

            try
            {
                dbCommand.ExecuteNonQuery();
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

        private static List<string> lMessages = new List<string>();

        public static dbAction queueDbAction(string sUserName, string sPassword, string sPublicKey, DateTime credat)
        {
            return createAction(sqlAction.createUser, @"INSERT INTO user_tab (uname, password, publickey, credat) VALUES('[uname]', '[password]', '[publickey]','[credat]');".Replace("[password]", sPassword).Replace("[uname]", sUserName).Replace("[publickey]", sPublicKey).Replace("[credat]", credat.ToShortDateString()));
        }

        public static dbAction queueDbAction(string sUsername, string sPassword)
        {
            return createAction(sqlAction.loginUser, @"SELECT * FROM user_tab WHERE uname = '[uname]' AND password = '[password]';".Replace("[uname]", sUsername).Replace("[password]", sPassword));
        }

        private static dbAction createAction(sqlAction action, string command, bool queueAction = true)
        {
            dbAction returnAction = new dbAction();
            returnAction.SqlAction = action;
            returnAction.Command = command;
            if (queueAction)
            {

                dbHelper.lSqlCommands.Add(returnAction);
            }
            return returnAction;
        }


        public static void executeQueryWithoutReturn(dbAction action)
        {
            if (dbCommand != null & connection != null)
            {
                //Everytime a query results in a exception the program gathers exception data and builds up a stacktrace
                //this causes heavy performanceload, since gathering the performance data 

                //When a user wants to login or create a new user, we check if there is an existing user already
                if (action.SqlAction == sqlAction.loginUser)
                {

                    NpgsqlDataReader read = dbCommand.ExecuteReader();
                    read.Close();

                    if (read.HasRows)
                    {
                        action.Status = 4;
                    }
                    else
                    {
                        action.Status = -4;
                    }

                    action.Handeled = true;
                    return;
                }

                if (action.SqlAction == sqlAction.createUser)
                {

                    dbAction checkExistance = createAction(sqlAction.loginUser, @"SELECT * FROM user_tab WHERE uname = '[uname]';".Replace("[uname]", action.ChatUser.NewUser.UserName), false);
                    dbCommand.CommandText = checkExistance.Command;

                    NpgsqlDataReader read = dbCommand.ExecuteReader();
                    read.Close();

                    if (read.HasRows)
                    {
                        action.Status = -3;
                        action.Handeled = true;
                        return;
                    }
                    else
                    {
                        dbAction createUser = createAction(sqlAction.createUser, @"INSERT INTO user_tab (uname, password, publickey, credat) VALUES('[uname]', '[password]', '[publickey]','[credat]');".Replace("[password]", action.ChatUser.NewUser.Password).Replace("[uname]", action.ChatUser.NewUser.UserName).Replace("[publickey]", action.ChatUser.NewUser.PublicKey).Replace("[credat]", DateTime.Now.ToShortDateString()), false);
                        dbCommand.CommandText = createUser.Command;
                        try
                        {
                            int iResult = dbCommand.ExecuteNonQuery();

                            if (iResult == 1)
                            {
                                action.Status = 3;
                            }
                            else
                            {
                                action.Status = -3;
                            }

                            action.Handeled = true;
                            return;

                        }
                        catch (Exception ex)
                        {
                            action.Status = -5;
                            action.Handeled = true;
                            Console.WriteLine("++ Query queue error: " + ex.Message);
                            return;
                        }
                    }
                }
            }
        }

    }
}
