using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        EmailService email = new EmailService();
        ApplicationDbContext db = new ApplicationDbContext();

        public async Task AssignmentNotification(int ticket, string user)
        {

            // send email to developer
            var msg = new IdentityMessage();
            msg.Body = "You are the assigned develper for ticket number " + ticket
                + "\nTicket Title: " + db.Tickets.Find(ticket).title;
            msg.Subject = "New BugTracker Assignment";
            msg.Destination = db.Users.Find(user).Email;
            
            await email.SendAsync(msg);


            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

        public void EditNotification(int ticket, string user)
        {


            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

        public void CommentNotification(int ticket, string user)
        {

            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }
        
        public void AttachmentNotification(int ticket, string user)
        {
            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }
    }
}