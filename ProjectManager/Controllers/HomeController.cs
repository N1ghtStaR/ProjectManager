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
            //////////////////////////////////////////////////////////////////
            //                                                              //
            //   Взима информация от базата данни единствено в случай, че   //
            //   има логнат потребител. Връца информацията получена от      //
            //   базата страницирана само когато се извежда цялостната      //
            //   информация ( без филтрация ).                              //
            //                                                              //
            //////////////////////////////////////////////////////////////////

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

                    return View(model);  //Връща информацията в деректен вид
                }

                foreach (Developer developer in developers)
                {
                    DeveloperViewModel developerModel = new DeveloperViewModel(developer);
                    model.Add(developerModel);
                }

                int pageSize = 4;                             // Брой записи на страница
                int pageNumber = (page ?? 1);                 // Номер на текуща страница
                int maxPages = model.Count / (pageSize - 1);  // Брой на всички страници

                ViewBag.Page = pageNumber;
                ViewBag.Max = maxPages;

                try
                {
                    return View(model.ToPagedList(pageNumber, pageSize));  //Връща информацията в странизиран вид  
                }
                catch // if ( pageNumber < 0 )
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