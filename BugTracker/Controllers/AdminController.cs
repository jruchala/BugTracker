using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BugTracker.Models;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper helper = new UserRolesHelper();

        // GET: Admin
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            List < AdminUserViewModel > = new List<AdminUserViewModel>();
            return View();
        }
    }
}