using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.Upload)]
        public string FileUrl { get; set; }

  
      

        // navigational properties
        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}