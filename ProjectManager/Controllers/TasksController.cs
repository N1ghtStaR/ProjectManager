namespace ProjectManager.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using ProjectManager.Filters;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;

    [IsAuthenticated]
    public class TasksController : Controller
    {
        private readonly UnitOfWork uow;

        public TasksController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
        }

        public TasksController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult FindTasks(int? id, string title)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Projects");
            }

            Project project = uow.ProjectRepository.GetProjectByID((int)id);

            Session["ProjectID"] = id;
            Session["ProjectTitle"] = title;
            Session["ProjectStatus"] = project.Status;

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return View(uow.TaskRepository.GetAllTaskForProject((int)Session["ProjectID"]));
        }

        public ActionResult Status (string status)
        {
            return View("Index", uow.TaskRepository.GetTasksByStatus((int)Session["ProjectID"], status));
        }

        public ActionResult Create()
        {
            if(Session["ProjectStatus"].ToString().Equals("Ready"))
            {
                return RedirectToAction("Index", "Task");
            }

            ViewBag.Owner = Session["ProjectID"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (ModelState.IsValid)
            {
                uow.TaskRepository.Create(task);
                uow.TaskRepository.Save();
            }

            ViewBag.Owner = Session["ProjectID"];

            return RedirectToAction("Index", "Tasks");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = uow.TaskRepository.GetTaskByID((int)id);

            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.Owner = Session["ProjectID"];

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (ModelState.IsValid)
            {
                uow.TaskRepository.Update(task);
                uow.TaskRepository.Save();

                return RedirectToAction("Index", "Tasks");
            }

            ViewBag.Owner = Session["ProjectID"];
            
            return View(task);
        }

        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Tasks");
            }

            Task task = uow.TaskRepository.GetTaskByID((int)id);

            if(task == null)
            {
                return HttpNotFound();
            }

            ViewBag.Owner = Session["ProjectID"];
            ViewBag.Status = "Ready";

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (ModelState.IsValid)
            {
                uow.TaskRepository.Update(task);
                uow.TaskRepository.Save();

                return RedirectToAction("Index", "Tasks");
            }

            return View(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.TaskRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
