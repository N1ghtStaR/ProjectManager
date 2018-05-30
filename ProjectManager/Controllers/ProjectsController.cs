namespace ProjectManager.Controllers
{
    using System;
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
    public class ProjectsController : Controller
    {
        private readonly UnitOfWork uow;

        public ProjectsController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
        }

        public ProjectsController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Index(string projectTitle)
        {
            IEnumerable<Project> projects = uow.ProjectRepository.GetAllProjectsForUser((int)Session["ID"]);

            List<ProjectViewModel> model = new List<ProjectViewModel>();

            foreach (Project project in projects)
            {
                ProjectViewModel projectModel = new ProjectViewModel(project);
                model.Add(projectModel);
            }

            if (!String.IsNullOrEmpty(projectTitle))
            {
                List<ProjectViewModel> modelSearch = new List<ProjectViewModel>();

                foreach(Project project in projects)
                {
                    if(project.Title.ToLower().Contains(projectTitle))
                    {
                        ProjectViewModel projectSearchModel = new ProjectViewModel(project);
                        modelSearch.Add(projectSearchModel);
                    }
                }

                return View(modelSearch);
            }

            return View(model);
        }
        
        public ActionResult Status(string status)
        {
            IEnumerable<Project> projects = uow.ProjectRepository.GetProjectsByStatus(status, (int)Session["ID"]);

            List<ProjectViewModel> model = new List<ProjectViewModel>();

            foreach(Project project in projects)
            {
                ProjectViewModel projectModel = new ProjectViewModel(project);
                model.Add(projectModel);
            }

            return View("Index", model);
        }

        public ActionResult Create()
        {
            ViewBag.Owner = Session["ID"];

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] ProjectViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project
                {
                    ID = projectModel.ID,
                    DeveleperID = projectModel.DeveleperID,
                    Title = projectModel.Title,
                    Description = projectModel.Description,
                    Category = (Project.Division)projectModel.Category,
                    Status = (Project.Statistic)projectModel.Status
                };

                uow.ProjectRepository.Create(project);
                uow.ProjectRepository.Save();

                return RedirectToAction("Index", "Projects");
            }

            return View(projectModel);
        }
       
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = uow.ProjectRepository.GetProjectByID((int)id);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel model = new ProjectViewModel(project);

            ViewBag.Status = "Ready";
            ViewBag.Owner = Session["ID"];

            Session["ProjectID"] = id;

            return View("Confirm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] ProjectViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Task> tasks = uow.TaskRepository.GetAllTaskForProject((int)projectModel.ID);

                if (tasks != null)
                {
                    foreach (Task task in tasks)
                    {
                        if (task.Status.ToString().Equals("InProgress"))
                        {
                            return RedirectToAction("Status", "Tasks", routeValues: new { ProjectID = projectModel.ID, Status = "InProgress" });
                        }
                    }
                }

                Project project = new Project
                {
                    ID = projectModel.ID,
                    DeveleperID = projectModel.DeveleperID,
                    Title = projectModel.Title,
                    Description = projectModel.Description,
                    Category = (Project.Division)projectModel.Category,
                    Status = (Project.Statistic)projectModel.Status
                };

                uow.ProjectRepository.Update(project);
                uow.ProjectRepository.Save();

                Session["ProjectID"] = project.ID;

                return RedirectToAction("Create", "Incomes");
            }

            ViewBag.Owner = Session["ID"];

            return View(projectModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = uow.ProjectRepository.GetProjectByID((int)id);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel model = new ProjectViewModel(project);

            ViewBag.Owner = Session["ID"];

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] ProjectViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project
                {
                    ID = projectModel.ID,
                    DeveleperID = projectModel.DeveleperID,
                    Title = projectModel.Title,
                    Description = projectModel.Description,
                    Category = (Project.Division)projectModel.Category,
                    Status = (Project.Statistic)projectModel.Status
                };

                uow.ProjectRepository.Update(project);
                uow.ProjectRepository.Save();

                return RedirectToAction("Index", "Projects");
            }

            ViewBag.Owner = Session["ID"];

            return View(projectModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = uow.ProjectRepository.GetProjectByID((int)id);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel model = new ProjectViewModel(project);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = uow.ProjectRepository.GetProjectByID(id);

            IEnumerable<Task> tasks = uow.TaskRepository.GetAllTaskForProject(id);

            foreach(Task task in tasks)
            {
                uow.TaskRepository.Delete(task);
                uow.TaskRepository.Save();
            }
            uow.ProjectRepository.Delete(project);
            uow.ProjectRepository.Save();

            return RedirectToAction("Index", "Projects");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.ProjectRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
