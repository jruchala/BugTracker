using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        TicketHistory ticketHistory = new TicketHistory();
        Ticket ticket = new Ticket();

        public void AddHistory(int ticketId, string updateProperty, string oldValue, string newValue, string userId)
        {
            ticketHistory.TicketId = ticketId;
            ticketHistory.Property = updateProperty;
            ticketHistory.OldValue = oldValue;
            ticketHistory.NewValue = newValue;
            ticketHistory.UserId = userId;
            ticketHistory.Changed = DateTime.Now;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }
    }
}