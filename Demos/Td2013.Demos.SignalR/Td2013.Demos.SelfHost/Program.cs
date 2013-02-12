using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;

namespace Td2013.Demos.SelfHost
{
    /// <summary>
    /// GET STARTED:
    /// Install-Package Microsoft.Owin.Hosting -pre
    ///Install-Package Microsoft.Owin.Host.HttpListener -pre
    ///Install-Package Microsoft.AspNet.SignalR.Owin -pre
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:20133";

            using (WebApplication.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}, ready to signal!", url);
                Console.WriteLine("==============================================");
                Console.WriteLine("");
                Console.ReadLine();
            }
        }

        class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.MapHubs();
            }
        }

        public class SelfHub : Hub
        {
            public void Send(UserMessage userMessage)
            {
                var log = string.Format("[{0}] : {1}", userMessage.Source, userMessage.Message);
                Console.WriteLine(log);
                Clients.Others.addMessage(userMessage);
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
}
