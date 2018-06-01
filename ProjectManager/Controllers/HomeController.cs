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

        public ActionResult Index(string developerUsername, int? page)
        {
            if(Session["ID"] != null)
            {
                IEnumerable<Developer> developers = uow.DeveloperRepository.GetAllDevelopers();

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

                    ///////////////////////////////////////////////////////
                    //                                                   //
                    // Когато се извежда информация използвайки търсачка //
                    //      получената информация не се страницира       //
                    //                                                   //
                    ///////////////////////////////////////////////////////

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

                //////////////////////////////////////////////////////
                //                                                  //
                // Когато се извежда цялостна информация за всички  //
                // регистрирани потребители, получената информация  //
                //                 се страницира                    //
                //                                                  //
                //////////////////////////////////////////////////////

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