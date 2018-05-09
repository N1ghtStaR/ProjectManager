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

        public ActionResult Index()
        {
            return View(db.Developers.ToList());
        }
    }
}