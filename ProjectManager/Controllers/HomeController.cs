namespace ProjectManager.Controllers
{
    using ProjectManager.Models;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;
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
                IEnumerable<Developer> developers = uow.DeveloperRepository.GetAllDevelopers();

                List<DeveloperViewModel> model = new List<DeveloperViewModel>();

                if (!String.IsNullOrEmpty(developerUsername))
                {
                    foreach(Developer developer in developers)
                    {
                        if(developer.Username.ToLower().Contains(developerUsername))
                        {
                            DeveloperViewModel developerSearchModel = new DeveloperViewModel(developer);
                            model.Add(developerSearchModel);
                        }
                    }

                    return View(model);
                }

                foreach (Developer developer in developers)
                {
                    DeveloperViewModel developerModel = new DeveloperViewModel(developer);
                    model.Add(developerModel);
                }

                return View(model);
            }

            return View();
        }
    }
}