using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using System.Runtime.Remoting.Messaging;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Projects
        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            //var nonProjectUsers = helper.ListUsersNotOnProject(project.Id);
            //var selectableUsers = helper.ListUsersNotOnProject(project.Id).ToArray();
            //var multiSelectUsers = new MultiSelectList(db.Users, "Name", "Name", selectableUsers);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Projects");
            }
            return View(project);
        }

        // GET: Projects/AssignUsers/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AssignUsers(int id)
        {
            Project project = db.Projects.Find(id);
            ProjectAssignHelper helper = new ProjectAssignHelper();
            var projectUser = new ProjectUserViewModel();
            projectUser.Project = project;
            projectUser.Id = project.Id;
            projectUser.Name = project.Name;
            projectUser.Users = db.Users.ToList();
            projectUser.SelectedUsers = helper.ListProjectUsers(projectUser.Id).Select(p => p.Id).ToArray();
            projectUser.ProjectUsers = new MultiSelectList(db.Users, "Id", "LastName", projectUser.SelectedUsers);

            return View(projectUser);
        }

        // POST: Projects/AssignUsers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsers(ProjectUserViewModel model)
        {                
            var project = db.Projects.Find(model.Id);
            foreach (var UserRm in db.Users.Select(r => r.Id).ToList())
            {
                helper.RemoveUserFromProject(UserRm, project.Id);
            }
            //foreach (var UserAdd in db.Users.Select(r => r.Id).ToList())
            foreach (var UserAdd in model.SelectedUsers)
            {
                helper.AddUserToProject(UserAdd, project.Id);
            }

            return RedirectToAction("Details", "Projects", new { id = model.Id });   
        }


        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
