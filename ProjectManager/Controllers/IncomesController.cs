namespace ProjectManager.Controllers
{
    using System.Web.Mvc;
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDataAccess.Repositories.IncomeRepository;
    using ProjectManagerDataAccess.Repositories.ProjectRepository;
    using ProjectManagerDB;

    using ProjectManagerDB.Entities;
    public class IncomesController : Controller
    {
        private IIncomeRepository incomeRepository;
        private IDeveloperRepository developerRepository;
        private IProjectRepository projectRepository;

        public IncomesController()
        {
            this.incomeRepository = new IncomeRepository(new ProjectManagerDbContext());
        }

        public IncomesController(IIncomeRepository incomeRepository, IDeveloperRepository developerRepository, IProjectRepository projectRepository)
        {
            this.incomeRepository = incomeRepository;
            this.developerRepository = developerRepository;
            this.projectRepository = projectRepository;
        }

        public ActionResult FindIncomes(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            Session["ProjectID"] = id;
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            int id = (int)Session["ID"];
            return View(incomeRepository.GetIncomesForUser(id));
        }

        public ActionResult Create()
        {
            int id = (int)Session["ProjectID"];

            Project project = projectRepository.GetProjectByID(id);

            ViewBag.ProjectName = project.Title;
            ViewBag.OwnerProject = Session["ProjectID"];
            ViewBag.OwnerDeveloper = Session["ID"];

            return View(projectRepository.GetProjectByID(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,DeveleperID,Title,Amount,ReleaseDate")] Income income)
        {
            if (ModelState.IsValid)
            {
                incomeRepository.Create(income);
                incomeRepository.Save();

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

            int ID = (int)Session["ID"];
            Income income = incomeRepository.GetIncomeByID(ID);

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
                incomeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
