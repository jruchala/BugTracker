using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectUserViewModel
    {
        public Project Project { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public MultiSelectList ProjectUsers { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser[] SelectedUsers { get; set; }
    }
}