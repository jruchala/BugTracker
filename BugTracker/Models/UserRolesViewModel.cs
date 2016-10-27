using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class UserRolesViewModel
    {
        public ApplicationUser User { get; set; }

        public List<string> Roles { get; set; }
        public MultiSelectList UserRoles { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}