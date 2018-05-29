namespace ProjectManager.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using ProjectManager.Filters;
    using ProjectManager.Models;
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

            return RedirectToAction("Index", "Tasks");
        }


        public ActionResult Index()
        {
            IEnumerable<Task> tasks = uow.TaskRepository.GetAllTaskForProject((int)Session["ProjectID"]);
            List<TaskViewModel> model = new List<TaskViewModel>();

            foreach(Task task in tasks)
            {
                TaskViewModel taskModel = new TaskViewModel(task);
                model.Add(taskModel);
            }

            return View(model);
        }
        
        public ActionResult Status (string status)
        {
            IEnumerable<Task> tasks = uow.TaskRepository.GetTasksByStatus((int)Session["ProjectID"], status);
            List<TaskViewModel> model = new List<TaskViewModel>();

            foreach(Task task in tasks)
            {
                TaskViewModel taskModel = new TaskViewModel(task);
                model.Add(taskModel);
            }

            return View("Index", model);
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
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,Priority,Status")] TaskViewModel taskModel)
        {
            if (ModelState.IsValid)
            {
                Task task = new Task
                {
                    ID = taskModel.ID,
                    ProjectID = taskModel.ProjectID,
                    Description = taskModel.Description,
                    Priority = (Task.Anteriority)taskModel.Priority,
                    Status = (Task.Stats)taskModel.Status
                };

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

            TaskViewModel model = new TaskViewModel(task);

            ViewBag.Owner = Session["ProjectID"];

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,Priority,Status")] TaskViewModel taskModel)
        {
            if (ModelState.IsValid)
            {
                Task task = new Task
                {
                    ID = taskModel.ID,
                    ProjectID = taskModel.ProjectID,
                    Description = taskModel.Description,
                    Priority = (Task.Anteriority)taskModel.Priority,
                    Status = (Task.Stats)taskModel.Status
                };

                uow.TaskRepository.Update(task);
                uow.TaskRepository.Save();
                
                return RedirectToAction("Index", "Tasks");
            }

            ViewBag.Owner = Session["ProjectID"];
            
            return View(taskModel);
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

            TaskViewModel model = new TaskViewModel(task);

            ViewBag.Owner = Session["ProjectID"];
            ViewBag.Status = "Ready";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,ProjectID,Description,Priority,Status")] TaskViewModel taskModel)
        {
            if (ModelState.IsValid)
            {
                Task task = new Task
                {
                    ID = taskModel.ID,
                    ProjectID = taskModel.ProjectID,
                    Description = taskModel.Description,
                    Priority = (Task.Anteriority)taskModel.Priority,
                    Status = (Task.Stats)taskModel.Status
                };

                uow.TaskRepository.Update(task);
                uow.TaskRepository.Save();

                return RedirectToAction("Index", "Tasks");
            }

            return View(taskModel);
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
