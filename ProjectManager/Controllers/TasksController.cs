using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagerDB;
using ProjectManagerDB.Entities;

namespace ProjectManager.Controllers
{
    public class TasksController : Controller
    {
        private ProjectManagerDbContext db = new ProjectManagerDbContext();

        public ActionResult FindTasks(int? id, string title)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Project project = db.Projects
                                    .Find(id);

            Session["ProjectID"] = id;
            Session["ProjectTitle"] = title;
            Session["ProjectStatus"] = project.Status;

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ProjectID"];

            var tasks = db.Tasks
                    .Where(t => t.ProjectID.Equals(id));

            return View(tasks.ToList());
        }

        public ActionResult ListInProgress()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            int id = (int)Session["ProjectID"];

            var tasks = db.Tasks
                    .Where(p => p.Status.ToString().Equals("InProgress"))
                    .Where(pp => pp.ProjectID.Equals(id));

            return View("List", tasks.ToList());
        }

        public ActionResult Ready()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ProjectID"];

            var tasks = db.Tasks
                   .Where(p => p.Status.ToString().Equals("Ready"))
                   .Where(d => d.ProjectID.Equals(id));

            return View("List", tasks.ToList());
        }

        public ActionResult Create()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if(Session["ProjectStatus"].ToString().Equals("Ready"))
            {
                return RedirectToAction("List", "Task");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }
            else
            {
                ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", task.ProjectID);
            }

            return RedirectToAction("List", "Tasks");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", task.ProjectID);

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", task.ProjectID);

            return RedirectToAction("List", "Tasks");
        }

        public ActionResult Confirm(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return RedirectToAction("List", "Tasks");
            }

            Task task = db.Tasks.Find(id);
            if(task == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", task.ProjectID);

            ViewBag.Status = "Ready";

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List", "Tasks");
            }

            return View(task);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();

            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);

            db.Tasks.Remove(task);
            db.SaveChanges();

            return RedirectToAction("Index");
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
