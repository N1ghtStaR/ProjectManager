using ProjectManagerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManager.Controllers
{
    public class HomeController : Controller
    {
        private ProjectManagerDbContext db = new ProjectManagerDbContext();

        public ActionResult Index(string developerUsername)
        {
            if(!String.IsNullOrEmpty(developerUsername))
            {
                return View(db.Developers
                                .Where(d => d.Username.Contains(developerUsername))
                                .ToList());
            }
            return View(db.Developers.ToList());
        }
    }
}