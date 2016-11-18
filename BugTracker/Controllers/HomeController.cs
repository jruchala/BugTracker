using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
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
    }
}