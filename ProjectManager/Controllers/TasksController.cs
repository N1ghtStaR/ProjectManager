namespace ProjectManager.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using ProjectManagerDataAccess.Repositories.ProjectRepository;
    using ProjectManagerDataAccess.Repositories.TaskRepository;
    using ProjectManagerDB;

    using ProjectManagerDB.Entities;
    public class TasksController : Controller
    {
        private ITaskRepository taskRepository;
        private IProjectRepository projectRepository;

        public TasksController()
        {
            this.taskRepository = new TaskRepository(new ProjectManagerDbContext());
            this.projectRepository = new ProjectRepository(new ProjectManagerDbContext());
        }

        public TasksController(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            this.taskRepository = taskRepository;
            this.projectRepository = projectRepository;
        }

        public ActionResult FindTasks(int? id, string title)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            int ID = (int)id;
            Project project = projectRepository.GetProjectByID(ID);

            Session["ProjectID"] = id;
            Session["ProjectTitle"] = title;
            Session["ProjectStatus"] = project.Status;

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ProjectID"];

            return View(taskRepository.GetAllTaskForProject(id));
        }

        public ActionResult Status (string status)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            int id = (int)Session["ProjectID"];

            return View("Index", taskRepository.GetTasksByStatus(status));
        }

        public ActionResult Create()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if(Session["ProjectStatus"].ToString().Equals("Ready"))
            {
                return RedirectToAction("List", "Task");
            }

            ViewBag.Owner = Session["ProjectID"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,Priority,Status")] Task task)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            if (ModelState.IsValid)
            {
                taskRepository.Create(task);
                taskRepository.Save();
            }
            else
            {
                ViewBag.Owner = Session["ProjectID"];
            }

            return RedirectToAction("List", "Tasks");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int ID = (int)id;
            Task task = taskRepository.GetTaskByID(ID);

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
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            if (ModelState.IsValid)
            {
                taskRepository.Update(task);
                taskRepository.Save();

                return RedirectToAction("Index", "Tasks");
            }

            ViewBag.Owner = Session["ProjectID"];
            
            return View(task);
        }

        public ActionResult Confirm(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return RedirectToAction("List", "Tasks");
            }

            int ID = (int)id;
            Task task = taskRepository.GetTaskByID(ID);
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
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                taskRepository.Update(task);
                taskRepository.Save();

                return RedirectToAction("Index", "Tasks");
            }

            return View(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                taskRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
