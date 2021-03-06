﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Text;

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
            msg.Subject = "New BugTracker Assignment";
            msg.Destination = db.Users.Find(user).Email;
            msg.Body = "You are the assigned develper for ticket number " + ticket
                + "\nTicket Title: " + db.Tickets.Find(ticket).title;
            
            await email.SendAsync(msg);


            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

        public async Task UnassignmentNotification(int ticket, string user)
        {

            var msg = new IdentityMessage();
            msg.Subject = "BugTracer Ticket Reassigned";
            msg.Destination = db.Users.Find(user).Email;
            msg.Body = String.Format("Ticket number {0} (Title: {1}) has been assigned to another developer. You are no longer responsible for this ticket.", ticket, db.Tickets.Find(ticket).title);

            await email.SendAsync(msg);

            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

        public async Task CommentNotification(int ticket, string user, string ticketTitle, string commentBody, string commentAuthor)
        {
            var msg = new IdentityMessage();
            msg.Subject = "New BugTracker Comment";
            msg.Destination = db.Users.Find(user).Email;
            msg.Body = String.Format(@"A comment has been posted to a ticket to which you are assigned.
                <br /> Ticket: {0}, {1}
                <br /> Comment: {2}
                <br /> Posted By: {3}", ticket, ticketTitle, commentBody, commentAuthor);
            await email.SendAsync(msg);
            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }
        
        public async Task AttachmentNotification(int ticket, string user, string ticketTitle, string fileName, string attacher)
        {
            var msg = new IdentityMessage();
            msg.Subject = "New BugTracker Attachment";
            msg.Destination = db.Users.Find(user).Email;
            msg.Body = String.Format(@"An attachment has been added to a ticket to which you are assigned.
                <br /> Ticket: {0}, {1}
                <br /> Attachment: {2}
                <br /> Added By: {3}", ticket, ticketTitle, fileName, attacher);

            await email.SendAsync(msg);
            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

        public async Task EditNotification(int ticket, string user, StringBuilder changes, string editor)
        {
            var msg = new IdentityMessage();
            msg.Subject = "BugTracker ticket has been edited";
            msg.Destination = db.Users.Find(user).Email;
            msg.Body = String.Format(@"Your BugTracker ticket, number {0}, has been edited.
                <br /> The following changes have been made:
                <br /> {1}
                <br /> by {2}", ticket, changes, editor);



            await email.SendAsync(msg);

            // create db record for notification
            var ticketNotification = new TicketNotification();
            ticketNotification.TicketId = ticket;
            ticketNotification.UserId = user;
            db.TicketNotifications.Add(ticketNotification);
            db.SaveChanges();
        }

    }
}