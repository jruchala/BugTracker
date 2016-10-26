using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketPriority
    {
        //TicketPriority properties
        public int Id { get; set; }
        public string Name { get; set; }

        // navigational properties
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }

    }
}