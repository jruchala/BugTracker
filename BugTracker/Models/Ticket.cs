using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string title { get; set; }
        [Display(Name ="Description")]
        public string description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        [Display(Name ="Priority")]
        public int TicketPriorityId { get; set; }
        [Display(Name = "Status")]
        public int TicketStatusId { get; set; }
        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }
        public string OwnerUserId { get; set; } // string because User ID is a string 
        public string AssignedToUserId { get; set; } // string because User ID is a string

        //Ticket Constructor

        public Ticket()
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();
        }
        

        // Navigational Properties for Children

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments  { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        //two connections to the User table to track Owner and AssignedTo

        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }


        // Navigational Property for Parent

        public virtual Project Project { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketType TicketType { get; set; }

    }
}