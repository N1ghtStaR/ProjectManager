namespace ProjectManager.Controllers
{
    using System.Web.Mvc;
    using ProjectManagerDataAccess;
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDataAccess.Repositories.IncomeRepository;
    using ProjectManagerDataAccess.Repositories.ProjectRepository;
    using ProjectManagerDB;

    using ProjectManagerDB.Entities;
    public class IncomesController : Controller
    {
        private readonly UnitOfWork uow;

        public IncomesController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
        }

        public IncomesController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult FindIncomes(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return HttpNotFound();
            }

            Session["ProjectID"] = id;

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            return View(uow.IncomeRepository.GetIncomesForUser((int)Session["ID"]));
        }

        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            Project project = uow.ProjectRepository.GetProjectByID((int)Session["ProjectID"]);

            ViewBag.ProjectName = project.Title;
            ViewBag.OwnerProject = Session["ProjectID"];
            ViewBag.OwnerDeveloper = Session["ID"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,DeveleperID,Title,Amount,ReleaseDate")] Income income)
        {
            if (ModelState.IsValid)
            {
                uow.IncomeRepository.Create(income);
                uow.IncomeRepository.Save();

                return RedirectToAction("Index");
            }

            ViewBag.OwnerDeveloper = Session["ID"];
            ViewBag.OwnerProject = Session["ProjectID"];
            return View(income);
        }

        public ActionResult Details(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if(id == null)
            {
                return RedirectToAction("List", "Projects");
            }

            Income income = uow.IncomeRepository.GetIncomeByID((int)id);

            if (income == null)
            {
                return HttpNotFound();
            }

            return View(income);
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
