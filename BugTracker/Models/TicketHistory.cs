using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        //TODO: Add Ticket History Properties
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Changed { get; set; }
        public string UserId { get; set; }


        // navigational properties
        public virtual ApplicationUser User { get; set; }
        public virtual Ticket ticket { get; set; }
    }
}