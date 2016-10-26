using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string UserId { get; set; }

    }
}