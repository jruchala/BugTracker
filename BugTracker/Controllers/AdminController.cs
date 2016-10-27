using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper helper = new UserRolesHelper();

        // GET: Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            List<UserRolesViewModel> userList = new List<UserRolesViewModel>();
            foreach (var users in db.Users.ToList())
            {
                var userCollection = new UserRolesViewModel();
                userCollection.User = users;
                userCollection.Roles = helper.ListUserRoles(users.Id).ToList();
                userList.Add(userCollection);
            }

            return View(userList);
        }

     
        }
    }
}