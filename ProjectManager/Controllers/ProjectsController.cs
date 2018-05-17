using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagerDB;
using ProjectManagerDB.Entities;

namespace ProjectManager.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectManagerDbContext db = new ProjectManagerDbContext();

        public ActionResult List(string projectName)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }   
            int id = (int)Session["ID"];

            if(!String.IsNullOrEmpty(projectName))
            {
                return View(db.Projects
                                .Where(p => p.DeveleperID.Equals(id))
                                .Where(s => s.Title.Contains(projectName))
                                .ToList());
            }

            return View(db.Projects
                            .Where(p => p.DeveleperID.Equals(id))
                            .ToList());
        }

        public ActionResult ListInProgress()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ID"];

            var projects = db.Projects
                                .Where(p => p.Status.ToString().Equals("InProgress"))
                                .Where(d => d.DeveleperID.Equals(id));

            return View("List", projects.ToList());
        }

        public ActionResult Ready()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ID"];

            var projects = db.Projects
                                .Where(p => p.Status.ToString().Equals("Ready"))
                                .Where(d => d.DeveleperID.Equals(id));

            return View("List", projects.ToList());
        }

        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", Session["ID"]);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Projects");
        }

        public ActionResult Confirm(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", project.DeveleperID);

            Session["ProjectID"] = id;
            ViewBag.Status = "Ready";

            return View("Confirm", project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                int id = (int)Session["ID"];
                var tasks = db.Tasks
                                .Where(p => p.ProjectID.Equals(id));

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        if (task.Status.ToString().Equals("InProgress"))
                        {
                            return RedirectToAction("List", "Tasks");
                        }
                    }
                }

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                Session["ProjectID"] = project.ID;

                return RedirectToAction("Create", "Incomes");
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", project.DeveleperID);

            return View(project);
        }

        public ActionResult Edit(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", project.DeveleperID);

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List", "Projects");
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", project.DeveleperID);

            return View(project);
        }

        public ActionResult Delete(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);

            db.Projects.Remove(project);
            db.SaveChanges();

            return RedirectToAction("List", "Projects");
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
