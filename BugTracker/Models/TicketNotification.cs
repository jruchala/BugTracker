using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        //TODO: Add Ticket notification properties
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        // navigational properties
        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}