using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglarR2Demo
{
    [HubName("ViewDataHub")]
    public class Chat : Hub
    {
        public string Hello()
        {
            return "Hello";
        }
        
        public void SendMessage(string message)
        {
            Clients.Others.talk(message);
        }

    }
}