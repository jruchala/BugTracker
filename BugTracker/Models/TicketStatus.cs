﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        //TicketStatus properties

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketStatus()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}