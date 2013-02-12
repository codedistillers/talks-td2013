using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Td2013.Demos.SignalR.Code
{
    [HubName("Notifier")]
    public class DefaultNotifierHub : Hub
    {

        public void Notify(string message)
        {
            Clients.Others.notified(message);
        }

        public void ConnectUser(string name, string connectionid,string list)
        {
            Groups.Add(Context.ConnectionId, list);
            Clients.Group(list).userConnected(name, connectionid);
        }

        public void DisconnectUser(string name, string connectionid, string list)
        {
            Groups.Remove(Context.ConnectionId, list);
            Clients.Group(list).userDisconnected(name, connectionid);
        }
    }

}