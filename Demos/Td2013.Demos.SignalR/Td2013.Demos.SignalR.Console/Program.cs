using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Td2013.Demos.SignalR.ShellApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var hub = new HubConnection("http://localhost:20133");

            IHubProxy client = hub.CreateHubProxy("SelfHub");
            client.On<UserMessage>("addmessage", 
                (msg) => Console.WriteLine("{0} : {1}", msg.Source,msg.Message));
            
            hub.Start().Wait();
            Console.WriteLine("Enter your name, then [enter]");

            var user = Console.ReadLine() ?? "guest";
            client["user"] = user;
            string line = null;
            while ((line = Console.ReadLine()) != null)
            {
                client.Invoke("Send", new UserMessage(user,line)).ContinueWith(
                    task =>
                        {
                            if(task.IsFaulted)
                                Console.WriteLine("ERR : {0}",task.Exception.GetBaseException());
                            else
                                Console.WriteLine("OK...Sent");
                        });
            }
        }

    }
    public class UserMessage
    {
        public string Source { get; set; }
        public string Message { get; set; }

        public UserMessage(string source, string message)
        {
            Source = source;
            Message = message;
        }
    }
}
