using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        //TODO: Add Ticket History Properties

        // navigational properties
        public virtual Ticket ticket { get; set; }
    }
}