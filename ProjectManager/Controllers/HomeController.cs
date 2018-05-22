using ProjectManagerDataAccess.Repositories.DeveloperRepository;
using ProjectManagerDB;
using ProjectManagerDB.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ProjectManager.Controllers
{
    public class HomeController : Controller
    {
        private IDeveloperRepository developerRepository;

        public HomeController()
        {
            this.developerRepository = new DeveloperRepository(new ProjectManagerDbContext());
        }

        public HomeController(IDeveloperRepository developerRepository)
        {
            this.developerRepository = developerRepository;
        }

        public ActionResult Index(string developerUsername)
        {
            if (!String.IsNullOrEmpty(developerUsername))
            {
                var developersSearched = from s in developerRepository.GetDevelopersByUsername(developerUsername) select s;

                return View(developersSearched);
            }

            var developers = from s in developerRepository.GetDevelopers() select s;

            return View(developers);
        }
    }
}