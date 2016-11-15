using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;
using System.Threading.Tasks;
using System.Text;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ProjectAssignHelper helper = new ProjectAssignHelper();
        public UserRolesHelper userHelper = new UserRolesHelper();
        HistoryHelper historyHelper = new HistoryHelper();


        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            { 
                var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                return View(tickets.ToList());
            }
            else if (User.IsInRole("Submitter"))
            {
                var user = db.Users.Find(User.Identity.GetUserId()).Id;
                var tickets = db.Tickets.Where(t => t.OwnerUserId == user).Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                return View(tickets.ToList());
            }
            else if (User.IsInRole("Developer"))
            {
                var user = db.Users.Find(User.Identity.GetUserId()).Id;
                var tickets = db.Tickets.Where(t => t.AssignedToUserId == user).Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                return View(tickets.ToList());
            }
            else if (User.IsInRole("Project Manager"))
            {
                var tickets = new List<Ticket>();
                var myProjects = helper.ListUserProjects(User.Identity.GetUserId());
                foreach (var project in myProjects)
                {
                    foreach (var ticket in project.Tickets)
                    {
                        tickets.Add(ticket);
                    }
                }
                return View(tickets.ToList());
            }
            else
            {
                return View();
            }

        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = db.Users.Find(User.Identity.GetUserId()).Id;
            ViewBag.UserObject = db.Users.Find(User.Identity.GetUserId());
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles ="Submitter")]
        public ActionResult Create(int? projectId)
        {
            if (projectId != null)
            {
                ViewBag.ProjectId = projectId;
            }
            else
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            }
            
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,title,description,Created,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                
                TicketHistory ticketHistory = new TicketHistory(); // create TicketHistory entry

                ticket.Created = DateTime.Now;
                ticket.OwnerUser= db.Users.Find(User.Identity.GetUserId());
                ticket.TicketStatusId = 1;
                db.Tickets.Add(ticket);
                
                ticketHistory.TicketId = ticket.Id;
                ticketHistory.Property = "Ticket Created";
                ticketHistory.UserId = ticket.OwnerUserId;
                ticketHistory.Changed = DateTime.Now;
                db.TicketHistories.Add(ticketHistory);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        


            ViewBag.OwnerUserId = ticket.OwnerUserId;
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var userId = db.Users.Find(User.Identity.GetUserId()).Id;
            var myProjects = helper.ListUserProjects(userId);
            var submitters = userHelper.UsersInRole("Submitter");
            var developers = userHelper.UsersInRole("Developers");
            ViewBag.AssignedToUserId = new SelectList(developers, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(submitters, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,description,Created,Updated,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // begin update history section
                var oldTicketInfo = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                // update db
                ticket.Updated = DateTime.Now;
                if (ticket.TicketStatusId == 1)
                {
                    ticket.AssignedToUserId = null;
                }
                

                if (oldTicketInfo.title != ticket.title)
                {
                    historyHelper.AddHistory(ticket.Id, "Title", oldTicketInfo.title, ticket.title, User.Identity.GetUserId());
                }

                if (oldTicketInfo.description != ticket.description)
                {
                    historyHelper.AddHistory(ticket.Id, "Description", oldTicketInfo.description, ticket.description, User.Identity.GetUserId());
                }

                if (oldTicketInfo.AssignedToUserId != ticket.AssignedToUserId
                    && oldTicketInfo.AssignedToUserId != null)
                {
                    var oldAssignedUser = db.Users.Find(oldTicketInfo.AssignedToUserId).FirstName
                        + " " + db.Users.Find(ticket.AssignedToUserId).LastName;
                    var newAssignedUser = db.Users.Find(ticket.AssignedToUserId).FirstName
                        + " " + db.Users.Find(ticket.AssignedToUserId).LastName;
                    historyHelper.AddHistory(ticket.Id, "Assigned User", oldAssignedUser, newAssignedUser, User.Identity.GetUserId());
                }

                if (oldTicketInfo.AssignedToUserId == null && oldTicketInfo.AssignedToUserId != ticket.AssignedToUserId)
                {
                    var oldAssignedUser = "Unassigned";
                    var newAssignedUser = db.Users.Find(ticket.AssignedToUserId).FirstName
                        + " " + db.Users.Find(ticket.AssignedToUserId).LastName;
                    historyHelper.AddHistory(ticket.Id, "Assigned User", oldAssignedUser, newAssignedUser, User.Identity.GetUserId());
                }

                if (oldTicketInfo.ProjectId != ticket.ProjectId)
                {
                    var oldProject = db.Projects.Find(oldTicketInfo.ProjectId).Name;
                    var newProject = db.Projects.Find(ticket.ProjectId).Name;
                    historyHelper.AddHistory(ticket.Id, "Project", oldProject, newProject, User.Identity.GetUserId());
                }

                if (oldTicketInfo.TicketPriorityId != ticket.TicketPriorityId)
                {
                    var oldPriority = db.TicketPriorities.Find(oldTicketInfo.TicketPriorityId).Name;
                    var newPriority = db.TicketPriorities.Find(ticket.TicketPriorityId).Name;
                    historyHelper.AddHistory(ticket.Id, "Priority", oldPriority, newPriority, User.Identity.GetUserId());
                }

                if (oldTicketInfo.TicketStatusId != ticket.TicketStatusId)
                {
                    var oldStatus = db.TicketStatuses.Find(oldTicketInfo.TicketStatusId).Name;
                    var newStatus = db.TicketStatuses.Find(ticket.TicketStatusId).Name;
                    historyHelper.AddHistory(ticket.Id, "Status", oldStatus, newStatus, User.Identity.GetUserId());
                }

                if (oldTicketInfo.TicketTypeId != ticket.TicketTypeId)
                {
                    var oldTicketType = db.TicketTypes.Find(oldTicketInfo.TicketTypeId).Name;
                    var newTicketType = db.TicketTypes.Find(ticket.TicketTypeId).Name;
                    historyHelper.AddHistory(ticket.Id, "Ticket Type", oldTicketType, newTicketType, User.Identity.GetUserId());
                }


                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property("title").IsModified = true;
                db.Entry(ticket).Property("description").IsModified = true;
                db.Entry(ticket).Property("Created").IsModified = false;
                db.Entry(ticket).Property("Updated").IsModified = true;
                db.Entry(ticket).Property("ProjectId").IsModified = true;
                db.Entry(ticket).Property("TicketPriorityId").IsModified = true;
                db.Entry(ticket).Property("TicketStatusId").IsModified = false;
                db.Entry(ticket).Property("TicketTypeId").IsModified = true;
                db.Entry(ticket).Property("OwnerUserId").IsModified = false;
                db.Entry(ticket).Property("AssignedToUserId").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

            return View(ticket);
        }


        // GET: Tickets/AssignDeveloper/5
        [HttpGet]
        [Authorize(Roles = "Project Manager")]
        public ActionResult AssignDeveloper(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            var devs = userHelper.UsersInRole("Developer");
            //ViewBag.AssignedToUserId = new SelectList(devs, "Id", "LastName", ticket.AssignedToUserId);

            ViewBag.AssignedToUserId = new SelectList(devs, "Id", "LastName");
            ViewBag.Project = ticket.Project.Name;
            ViewBag.Title = ticket.title;
            ViewBag.Description = ticket.description;
            return View(ticket);
        }

        // POST: Tickets/AssignDeveloper/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDeveloper([Bind(Include = "Id,title,description,Created,Updated,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Attach(ticket);
                ticket.TicketStatusId = 2;
                db.Entry(ticket).Property("title").IsModified = false;
                db.Entry(ticket).Property("description").IsModified = false;
                db.Entry(ticket).Property("Updated").IsModified = false;
                db.Entry(ticket).Property("ProjectId").IsModified = false;
                db.Entry(ticket).Property("TicketPriorityId").IsModified = false;
                db.Entry(ticket).Property("TicketStatusId").IsModified = true;
                db.Entry(ticket).Property("TicketTypeId").IsModified = false;
                db.Entry(ticket).Property("OwnerUserId").IsModified = false;
                db.Entry(ticket).Property("AssignedToUserId").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
            
            //ticket.TicketStatusId = 2;
            //db.Entry(ticket).State = EntityState.Modified;
            //db.SaveChanges();
            //return View("Index", "Tickets");
        }


        // GET: Tickets/Resolve/5
        [Authorize(Roles = "Developer, Project Manager")]
        public ActionResult Resolve(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketTitle = ticket.title;
            return View(ticket);
        }

        // POST: Tickets/Resolve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resolve([Bind(Include = "Id, title, description, Created, Updated, ProjectId, TicketPriorityId, TicketStatusId, TicketTypeId, OwnerUserId, AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Attach(ticket);
                ticket.TicketStatusId = 3;
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).Property("title").IsModified = false;
                db.Entry(ticket).Property("description").IsModified = false;
                db.Entry(ticket).Property("Created").IsModified = false;
                db.Entry(ticket).Property("Updated").IsModified = true;
                db.Entry(ticket).Property("ProjectId").IsModified = false;
                db.Entry(ticket).Property("TicketPriorityId").IsModified = false;
                db.Entry(ticket).Property("TicketStatusId").IsModified = true;
                db.Entry(ticket).Property("TicketTypeId").IsModified = false;
                db.Entry(ticket).Property("OwnerUserId").IsModified = false;
                db.Entry(ticket).Property("AssignedToUserId").IsModified = false;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(ticket);
        }
        // NO Delete Ticket Method, per SRS
        // GET: Tickets/Delete/5
        // POST: Tickets/Delete/5

    }
}
