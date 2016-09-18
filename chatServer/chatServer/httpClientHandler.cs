using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

namespace chatServer
{
    public enum httpServerCommands
    {
        generateSessionSeed = 0,
        createUser = 1,
        loginUser = 2,
        sendMessage = 3,
    }

    class httpClientHandler
    {
        public httpClientHandler(HttpListenerContext context)
        {
            HandleRequestAsync(context);
        }

        HttpListenerResponse response;
        string smessage;
        string sClassToCreate;

        private async Task HandleRequestAsync(HttpListenerContext context)
        {

            // Do processing here, possibly affecting KeepGoing to make the 
            // server shut down.
            byte[] bInput = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(bInput, 0, bInput.Length);
            smessage = Encoding.UTF8.GetString(bInput, 0, bInput.Length);
            //await Task.Delay(1000);

            if (!context.Request.ContentType.StartsWith("[serverCommand]")) { return; }
            //sClassToCreate = context.Request.ContentType.Replace("[serverCommand]", "");


            dynamic clientMessage = JsonConvert.DeserializeObject(smessage);
            Console.WriteLine(clientMessage.ServerCommand);

            string responseString = "";

            switch (((httpServerCommands)Convert.ToInt32(clientMessage.ServerCommand)))
            {
                case httpServerCommands.generateSessionSeed:
                    responseString = @"{'SessionSeed': '" + Guid.NewGuid().ToString() + "'}";
                    break;

                case httpServerCommands.createUser:
                    //JsonClasses.createUserJson newUser = JsonConvert.DeserializeObject<JsonClasses.createUserJson>()
                    JsonClasses.createUserJson newUser = ((JsonClasses.createUserJson)JsonConvert.DeserializeObject<JsonClasses.createUserJson>(smessage));
                    dbHelper.queueCreateUser(newUser.NewUser.UserName, newUser.NewUser.PublicKey, DateTime.Now);
                    break;
            }

            response = context.Response;
            
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // You must close the output stream.
            output.Close();


            //Perform(context);
        }

        private void Perform(HttpListenerContext ctx)
        {

            HttpListenerResponse response = ctx.Response;
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // You must close the output stream.
            output.Close();
        }
    }
}
