namespace ProjectManager.Controllers
{
    using ProjectManager.Models;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using PagedList;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Data;
    using System.Web.Services;
    using System.Configuration;
    using System.Data.SqlClient;

    public class HomeController : Controller
    {
        private readonly UnitOfWork uow;

        public HomeController()
        {
            uow = new UnitOfWork(new ProjectManagerDbContext());
        }

        public HomeController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Index(string developerUsername, int? page)
        {
            if(Session["ID"] != null)
            {
                IEnumerable<Developer> developers = uow.DeveloperRepository.GetAllDevelopers((int)Session["ID"]);

                List<DeveloperViewModel> model = new List<DeveloperViewModel>();

                if (!String.IsNullOrEmpty(developerUsername))
                {
                    foreach(Developer developer in developers)
                    {
                        if(developer.Username.Contains(developerUsername))
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

                int pageSize = 4;                            
                int pageNumber = (page ?? 1);                 
                int maxPages = model.Count / (pageSize - 1);  

                ViewBag.Page = pageNumber;
                ViewBag.Max = maxPages;

                try
                {
                    return View(model.ToPagedList(pageNumber, pageSize));  
                }
                catch
                {
                    page = 1;
                    pageNumber = (int)page;

                    ViewBag.Page = pageNumber;

                    return View(model.ToPagedList(pageNumber, pageSize));
                }
            }

            return View();
        }
    }
}