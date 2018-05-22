namespace ProjectManager.Controllers
{
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private IDeveloperRepository developerRepository;
        public HomeController() => this.developerRepository = new DeveloperRepository(new ProjectManagerDbContext());
        public HomeController(IDeveloperRepository developerRepository) => this.developerRepository = developerRepository;

        public ActionResult Index(string developerUsername)
        {
            if (!String.IsNullOrEmpty(developerUsername))
            { 
                return View(developerRepository.GetDevelopersByUsername(developerUsername));
            }
            return View(developerRepository.GetAllDevelopers());
        }
    }
}