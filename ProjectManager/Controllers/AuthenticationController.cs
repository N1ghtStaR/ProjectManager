using System.Collections.Generic;
using System.Data;
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
                    return RedirectToAction("Index");
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
                    var developer = db.Developers.Where(d => d.Username.Equals(username) && d.Password.Equals(password)).Single();

                    Session["ID"] = developer.ID;
                    Session["Username"] = developer.Username;
                    Session["Role"] = developer.Role;

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
        public ActionResult Promote(int? id) //Not Working HTTP 404!
        {
            Developer developer = db.Developers.Find(id);
            developer.Role.Equals("TeamLeader");
            UpdateModel(developer);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult Demote(int id) //Not Working HTTP 404!
        {
            Developer developer = db.Developers.Find(id);
            developer.Role.Equals("Developer");
            UpdateModel(developer);
            db.SaveChanges();
            return View(developer);
        }

        public ActionResult LogOut()
        {
            Session["ID"] = null;
            Session["Username"] = null;
            Session["Role"] = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccountDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.Developers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
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
