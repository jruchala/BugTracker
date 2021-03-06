﻿using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ProjectAssignHelper helper = new ProjectAssignHelper();

        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            if (user != null)
            {
                ViewBag.UserName = db.Users.Find(user).FirstName + " " + db.Users.Find(user).LastName;
                var ticket = db.Tickets;
                ViewBag.ProjectCount = db.Projects.Count();
                ViewBag.TicketCount = db.Tickets.Count();
                ViewBag.ResolvedCount = db.Tickets.Where(t => t.TicketStatusId == 3).Count();
                ViewBag.OpenCount = db.Tickets.Where(t => t.TicketStatusId <= 2).Count();
                ViewBag.UserCount = db.Users.Count();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [Authorize]
        public ActionResult ProjectList()
        {
            var user = User.Identity.GetUserId();
            var projects = helper.ListUserProjects(user);
            return View(projects.ToList());
           

        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult GetChart()
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var tickets = db.Tickets.Count();
                var ticketsResolved = db.Tickets.Where(t => t.TicketStatusId == 3).Count();
                var ticketsOpen = db.Tickets.Where(t => t.TicketStatusId == 2).Count();
                var ticketsUnassigned = db.Tickets.Where(t => t.TicketStatusId == 1).Count();

                var data = new[] 
                {
                    new { label = "Unassigned", value = ticketsUnassigned},
                    new { label = "Resolved", value = ticketsResolved},
                    new { label = "Open", value = ticketsOpen }
                };

                return Content(JsonConvert.SerializeObject(data), "application/json");
            }

            return View("Index", "Home");
        }
    }
}