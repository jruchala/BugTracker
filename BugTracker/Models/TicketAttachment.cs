using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        //TODO: Add TicketAttachment properties
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }

  
      

        // navigational properties
        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}