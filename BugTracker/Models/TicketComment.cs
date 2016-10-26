using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComment
    {
        //TODO: Add Ticket Comment properties
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public int TicketId { get; set; }

        // navigational properties
        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}