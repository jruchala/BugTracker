using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectAssignHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = project.Users.Any(u => u.Id == userId);
            return user;
        }

        public void AddUserToProject(string userId, int projectId)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project project = db.Projects.Find(projectId);
            project.Users.Add(user);
            db.SaveChanges();
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project project = db.Projects.Find(projectId);
            project.Users.Remove(user);
            db.SaveChanges();
        }

        public List<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            return user.Projects.ToList();
        }

        public List<ApplicationUser> ListProjectUsers(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            return project.Users.ToList();
        }

        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var userObj = project.Users;
            return db.Users.Where(u => !userObj.Contains(u)).ToList();
        }


    }
}