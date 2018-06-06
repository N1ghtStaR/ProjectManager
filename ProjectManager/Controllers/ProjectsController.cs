namespace ProjectManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using ProjectManager.Filters;
    using ProjectManager.Models;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using PagedList;
    using ProjectManagerFactory;

    [IsAuthenticated]
    public class ProjectsController : Controller
    {
        private readonly UnitOfWork uow;
        private readonly Factory factory;

        public ProjectsController()
        {
            uow = new UnitOfWork(new ProjectManagerDbContext());
            factory = new Factory();
        }

        public ProjectsController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Index(string projectTitle, int? page)
        {
            IEnumerable<Project> projects = uow.ProjectRepository.GetAllProjectsForUser((int)Session["ID"]);

            List<ProjectViewModel> model = new List<ProjectViewModel>();

            if (!String.IsNullOrEmpty(projectTitle))
            {

                foreach (Project project in projects)
                {
                    if (project.Title.ToLower().Contains(projectTitle))
                    {
                        ProjectViewModel projectSearchModel = new ProjectViewModel(project);
                        model.Add(projectSearchModel);
                    }
                }

                return View(model);
            }

            foreach (Project project in projects)
            {
                ProjectViewModel projectModel = new ProjectViewModel(project);
                model.Add(projectModel);
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
                try
                {
                    Project project = factory.ProjectFactory.New(projectModel);

                    uow.ProjectRepository.Create(project);
                    uow.ProjectRepository.Save();
                }
                catch
                {
                    ModelState.AddModelError("", "Database error! Unable to save changes. Try again later!");

                    return View(projectModel);
                }

                TempData["Message"] = "New project have successfully added!";

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
                            TempData["Message"] = "Project '" + projectModel.Title + "' have unfinished tasks!";

                            return RedirectToAction("Status", "Tasks", routeValues: new { ProjectID = projectModel.ID, Status = "InProgress" });
                        }
                    }
                }

                try
                {
                    Project project = factory.ProjectFactory.New(projectModel);

                    uow.ProjectRepository.Update(project);
                    uow.ProjectRepository.Save();

                    Session["ProjectID"] = project.ID;
                }
                catch
                {
                    ModelState.AddModelError("", "Database error! Enable to edit user's account. Try again later!");

                    return View("Confirm", projectModel);
                }

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
                try
                {
                    Project project = factory.ProjectFactory.New(projectModel);

                    uow.ProjectRepository.Update(project);
                    uow.ProjectRepository.Save();
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later!");

                    return View(projectModel);
                }

                TempData["Message"] = "Project '" + projectModel.Title + "' have been successfully updated!";

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

            try
            {
                if (tasks != null)
                {
                    foreach (Task task in tasks)          
                    {                                     
                        uow.TaskRepository.Delete(task);   
                        uow.TaskRepository.Save();        
                    }                                     
                }                                         

                uow.ProjectRepository.Delete(project);
                uow.ProjectRepository.Save();
            }
            catch
            {
                ModelState.AddModelError("", "Unable to delete this project right now!");

                return View("Confirm", project);
            }

            TempData["Message"] = "Project '" + project.Title + "' have been successfully deleted!";

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
