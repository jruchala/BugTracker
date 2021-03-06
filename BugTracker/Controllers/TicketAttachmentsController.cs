﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using BugTracker.Helpers;

namespace BugTracker.Models
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        NotificationHelper notificationHelper = new NotificationHelper();
        

        // GET: TicketAttachments
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize]
        public ActionResult Create(int id)
        {

            ViewBag.TicketId = id;
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TicketId,Description,Created,UserId,FileUrl")] TicketAttachment ticketAttachment, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                if (FileUploadValidator.IsWebFriendlyFile(attachment))
                {
                    var fileName = Path.GetFileName(attachment.FileName);
                    attachment.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads"), fileName));
                    ticketAttachment.FileUrl = "~/Content/Uploads/" + fileName;
                    ticketAttachment.Created = DateTime.Now;
                    db.TicketAttachments.Add(ticketAttachment);
                    db.SaveChanges();

                    // begin notification functionality
                    var ticket = db.Tickets.Find(ticketAttachment.TicketId);
                    var attacher = db.Users.Find(ticketAttachment.UserId).FirstName + " " + db.Users.Find(ticketAttachment.UserId).LastName;
                    var assignedUserId = ticket.AssignedToUserId;
                    if (assignedUserId != null)
                    {
                        await notificationHelper.AttachmentNotification(ticketAttachment.TicketId, assignedUserId, ticket.title, fileName, attacher);
                    }
                    // end notification
                    return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId});
                }
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Created,TicketId,UserId,FileUrl")] TicketAttachment ticketAttachment, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                if (attachment != null && attachment.ContentLength > 0)
                {
                    if (FileUploadValidator.IsWebFriendlyFile(attachment))
                    {
                        var fileName = Path.GetFileName(attachment.FileName);
                        attachment.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads"), fileName));
                        ticketAttachment.FileUrl = "~/Content/Uploads/" + fileName;
                        db.TicketAttachments.Add(ticketAttachment);
                    }
                }
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId});
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
