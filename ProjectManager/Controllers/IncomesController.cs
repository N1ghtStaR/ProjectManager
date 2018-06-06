namespace ProjectManager.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ProjectManager.Filters;
    using ProjectManager.Models;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using PagedList;
    using ProjectManagerFactory;

    [IsAuthenticated]
    public class IncomesController : Controller
    {
        private readonly UnitOfWork uow;
        private readonly Factory factory;

        public IncomesController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
            this.factory = new Factory();
        }

        public IncomesController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Index(int? page)
        {
            IEnumerable<Income> incomes = uow.IncomeRepository.GetIncomesForUser((int)Session["ID"]);

            if(incomes != null)
            {
                List<IncomeViewModel> model = new List<IncomeViewModel>();

                foreach(Income income in incomes)
                {
                    IncomeViewModel incomeModel = new IncomeViewModel(income);
                    model.Add(incomeModel);
                }

                int pageSize = 3;
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

        public ActionResult Create()
        {
            Project project = uow.ProjectRepository.GetProjectByID((int)Session["ProjectID"]);

            ViewBag.ProjectName = project.Title;
            ViewBag.OwnerProject = Session["ProjectID"];
            ViewBag.OwnerDeveloper = Session["ID"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,DeveleperID,Title,Amount,ReleaseDate")] IncomeViewModel incomeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Income income = factory.IncomeFactory.New(incomeModel);

                    uow.IncomeRepository.Create(income);
                    uow.IncomeRepository.Save();
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to add new income right now! Try again later!");

                    return View(incomeModel);
                }

                return RedirectToAction("Index");
            }

            ViewBag.OwnerDeveloper = Session["ID"];
            ViewBag.OwnerProject = Session["ProjectID"];

            return View(incomeModel);
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("List", "Projects");
            }

            Income income = uow.IncomeRepository.GetIncomeByID((int)id);
           
            if (income == null)
            {
                return HttpNotFound();
            }

            IncomeViewModel model = new IncomeViewModel(income);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.IncomeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
