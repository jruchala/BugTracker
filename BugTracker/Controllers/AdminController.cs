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
            UserRolesViewModel AdminModel = new UserRolesViewModel();
            UserRolesHelper helper = new UserRolesHelper();
            var selected = helper.ListUserRoles(id);
            AdminModel.UserRoles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.Id = user.Id;
            AdminModel.DisplayName = user.FirstName + " " + user.LastName;

            return View(AdminModel);
        }

        // POST: Admin/ManageRoles/5 

        public ActionResult ManageRoles(UserRolesViewModel model)
        {
            var user = db.Users.Find(model.Id);
            UserRolesHelper helper = new UserRolesHelper();
            foreach (var roleRm in db.Roles.Select(r => r.Id).ToList())
            {
                helper.RemoveUserFromRole(user.Id, roleRm);
            }

            foreach (var roleAdd in db.Roles.Select(r => r.Id).ToList())
            {
                helper.AddUserToRole(user.Id, roleAdd);
            }

            return RedirectToAction("Index");
        }

    }
}

