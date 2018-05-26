namespace ProjectManager.Controllers
{
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly UnitOfWork uow;

        public HomeController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
        }

        public HomeController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Index(string developerUsername)
        {
            if(Session["ID"] != null)
            {
                if (!String.IsNullOrEmpty(developerUsername))
                {
                    return View(uow.DeveloperRepository.GetDevelopersByUsername(developerUsername));
                }

                return View(uow.DeveloperRepository.GetAllDevelopers());
            }

            return View();
        }
    }
}