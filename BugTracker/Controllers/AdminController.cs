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
        [Authorize(Roles = "Admin")]
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

        // GET: Admin/ManageRoles/5

        public ActionResult ManageRoles(string id)
        {
            var user = db.Users.Find(id);
            var roleUser = new UserRolesViewModel();
            UserRolesHelper helper = new UserRolesHelper();
            roleUser.Id = user.Id;
            roleUser.FirstName = user.FirstName;
            roleUser.LastName = user.LastName;
            roleUser.DisplayName = user.FirstName + " " + user.LastName;
            roleUser.SelectedRoles = helper.ListUserRoles(user.Id).ToArray();
            roleUser.UserRoles = new MultiSelectList(db.Roles, "Name", "Name", roleUser.SelectedRoles);

            return View(roleUser);
        }

        // POST: Admin/ManageRoles/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles (UserRolesViewModel model)
        {
            var user = db.Users.Find(model.Id);
            foreach (var rolemv in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, rolemv);
            }
            foreach (var rolemv in model.SelectedRoles)
            {
                helper.AddUserToRole(user.Id, rolemv);
            }
            ViewBag.confirm = "User's role has been modified";

            return RedirectToAction("Index");
        }

        // POST: Admin/ManageRoles/5 

        //public ActionResult ManageRoles(UserRolesViewModel model)
        //{
        //    var user = db.Users.Find(model.Id);
        //    UserRolesHelper helper = new UserRolesHelper();
        //    foreach (var roleRm in db.Roles.Select(r => r.Id).ToList())
        //    {
        //        helper.RemoveUserFromRole(user.Id, roleRm);
        //    }

        //    foreach (var roleAdd in db.Roles.Select(r => r.Id).ToList())
        //    {
        //        helper.AddUserToRole(user.Id, roleAdd);
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}

