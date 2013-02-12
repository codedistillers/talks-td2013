using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Td2013.Demos.SignalR.Models
{
    public class CreatePriorityListCommand
    {
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public bool AllowNamedOnlyUsers { get; set; }

        public CreatePriorityListCommand(string currentUser = "")
        {
            OwnerName = currentUser;
        }

        public CreatePriorityListCommand()
        {
            OwnerName = "guest";
        }
    }

    public class JoinPriorityListCommand
    {
        public string BoardName { get; set; }
        public string UserName { get; set; }
    }


    public class PriorityList
    {
        public PriorityList()
        {
            List = new SortedList<int, string>();
            ConnectionIds = new Dictionary<string, string>();
        }

        public PriorityList(string name,string owner)
        {
            Name = name;
            OwnerName = owner;
            List = new SortedList<int, string>();
            ConnectionIds = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string CurrentUser { get; set; }
        public SortedList<int, string> List { get; set; }

        public Dictionary<string,string> ConnectionIds { get; set; } 
    }

    public class PriorityListViewModel
    {
        public PriorityList List { get; set; }
        public string UserName { get; set; }
    }


    public interface IPriorityListRepo
    {
        PriorityList GetOrCreate(string name,string owner);
        void Save(PriorityList list);
        void SaveItem(string list, string value);
        void SortItemOnList(string list, int origin, int newIndex);
    }

    public class PriorityListRepo : IPriorityListRepo
    {
        private static Dictionary<string, PriorityList> staticFakeList = new Dictionary<string, PriorityList>();

        public PriorityList GetOrCreate(string name,string owner)
        {
            PriorityList list;
            if (!staticFakeList.ContainsKey(name))
            {
                list = new PriorityList(name, owner);
                staticFakeList.Add(name, list);
            }
            list = staticFakeList[name];
            list.CurrentUser = owner;
            return list;
        }

        public void Save(PriorityList list)
        {
            if (staticFakeList.ContainsKey(list.Name))
                staticFakeList.Remove(list.Name);
            staticFakeList.Add(list.Name, list);
        }

        public void SaveItem(string list, string value)
        {
            if (!staticFakeList.ContainsKey(list))
            {
                return;
            }
            var board = staticFakeList[list];
            var id = board.List.Keys.Count;
            board.List.Add(id,value);

        }
        public void SortItemOnList(string list, int origin, int newIndex)
        {
            if (!staticFakeList.ContainsKey(list))
            {
                return;
            }
            var board = staticFakeList[list];
            var valueNewIndex = board.List[newIndex];
            board.List[newIndex] = board.List[origin];
            board.List[origin] = valueNewIndex;
        }
    }

}