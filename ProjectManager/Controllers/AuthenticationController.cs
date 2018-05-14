using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagerDB;
using ProjectManagerDB.Entities;

namespace ProjectManager.Controllers
{
    public class AuthenticationController : Controller
    {
        private ProjectManagerDbContext db = new ProjectManagerDbContext();

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "ID,Username,Email,Password,Role")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                    db.Developers.Add(developer);
                    db.SaveChanges();

                    Session["ID"] = developer.ID;
                    Session["Username"] = developer.Username;
                    Session["Role"] = developer.Role;
                    Session["ProjectID"] = null;

                return RedirectToAction("Index", "Home");
            }
            return View(developer);
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var developer = db.Developers
                                        .Where(d => d.Username.Equals(username) && d.Password.Equals(password))
                                        .Single();

                    Session["ID"] = developer.ID;
                    Session["Username"] = developer.Username;
                    Session["Role"] = developer.Role;
                    Session["ProjectID"] = null;

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,Username,Password,Email,Role")] Developer developer)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(developer).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            Session["ID"] = null;
            Session["Username"] = null;
            Session["Role"] = null;
            Session["ProjectID"] = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccountDetails(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Developer developer = db.Developers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }

            if (developer.Role.ToString().Equals("Developer"))
            {
                ViewBag.SubmitValue = "Promote";
                ViewBag.RoleValue = "TeamLeader";
            }
            else
            {
                ViewBag.SubmitValue = "Demote";
                ViewBag.RoleValue = "Developer";
            }

            return View(developer);
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
