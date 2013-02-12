using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Td2013.Demos.SignalR.Code;
using Td2013.Demos.SignalR.Models;

namespace Td2013.Demos.SignalR.Controllers
{
    public class PrioritiesController : Controller
    {
        private IPriorityListRepo repo;
        public PrioritiesController(IPriorityListRepo repo)
        {
            this.repo = repo;
            if (repo.GetOrCreate("default","guest").List.Count == 0)
            {
                var priorities = new PriorityList("default","demo");
                priorities.List.Add(0,"The First");
                priorities.List.Add(1, "The Second");
                priorities.List.Add(2, "The Third");
                repo.Save(priorities);
            }
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            return View(new CreatePriorityListCommand());
        }

        [HttpPost]
        public ActionResult CreateNew(CreatePriorityListCommand boardInfo)
        {
            if (ModelState.IsValid)
            {
                var board = repo.GetOrCreate(boardInfo.Title,boardInfo.OwnerName);
                //return RedirectToAction("EnterBoard",board);
                return RedirectToAction(
                    "List",
                    new JoinPriorityListCommand{BoardName = board.Name,UserName = board.CurrentUser});
            }
            return View();

        }
        [HttpGet]
        public ActionResult JoinBoard(string name = "default")
        {
            return View(new JoinPriorityListCommand{BoardName = name});
        }

        [HttpPost]
        public ActionResult JoinBoard(JoinPriorityListCommand command)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("List", command);
            }
            return View();
        }

        public ActionResult List(JoinPriorityListCommand command)
        {
            var board = repo.GetOrCreate(command.BoardName, command.UserName);
                
            return View(board);
        }
    }
}
