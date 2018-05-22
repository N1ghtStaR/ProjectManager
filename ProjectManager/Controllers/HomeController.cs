namespace ProjectManager.Controllers
{
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDB;
    using System;
    using System.Linq;
    using System.Web.Mvc;

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
                var developersSearched = from entities 
                                         in developerRepository.GetDevelopersByUsername(developerUsername)
                                         select entities;

                return View(developersSearched);
            }

            var developers = from entities 
                             in developerRepository.GetDevelopers()
                             select entities;

            return View(developers);
        }
    }
}