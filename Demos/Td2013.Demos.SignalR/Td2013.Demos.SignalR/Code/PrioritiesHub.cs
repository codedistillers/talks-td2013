using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Td2013.Demos.SignalR.Models;

namespace Td2013.Demos.SignalR.Code
{
    [HubName("Priorities")]
    public class PrioritiesHub : Hub
    {
        public string CurrentUser { get; set; }

        private IPriorityListRepo repo;

        
        public PrioritiesHub(IPriorityListRepo repo)
        {
            this.repo = repo;
        }

        
        public void Join(string list, string user)
        {
            Groups.Add(Context.ConnectionId, list);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultNotifierHub>();
            context.Clients.All.ConnectUser(user, Context.ConnectionId, list);
        }

        public void Leave(string list, string user)
        {
            Groups.Remove(Context.ConnectionId, list);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultNotifierHub>();
            context.Clients.All.DisconnectUser(user, Context.ConnectionId, list);
        }
        
        private SortedList<int, string> priorityList = new SortedList<int, string>();


        public void ToggleNewItem(string list,string value,string user)
        {
            repo.SaveItem(list,value);
            Clients.Group(list).addNewButtonClicked(user);
        }

        public void EnterKey(string list,string key,string user)
        {
            Clients.Group(list).keyDown(key, user);
        }

        public void SortItem(string list,string user, int origin, int newIndex)
        {
            repo.SortItemOnList(list, origin, newIndex);
            Clients.Group(list).itemSorted(origin, newIndex,user);
        }
    }

}