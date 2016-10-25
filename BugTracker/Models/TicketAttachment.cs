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
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string FileUrl { get; set; }
        public int TicketId { get; set; }

        //TODO: make sense of this
      

        // navigational properties
        public virtual Ticket ticket { get; set; }
    }
}