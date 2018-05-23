namespace ProjectManager.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;

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
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            
            int id = (int)Session["ID"];

            if(!String.IsNullOrEmpty(projectTitle))
            {
                return View(uow.ProjectRepository.GetProjectsByTitle(projectTitle, id));
            }

            return View(uow.ProjectRepository.GetAllProjectsForUser(id));
        }
        
        public ActionResult Status(string status)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            int id = (int)Session["ID"];

            return View("Index", uow.ProjectRepository.GetProjectsByStatus(status, id));
        }


        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            ViewBag.Owner = Session["ID"];

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                uow.ProjectRepository.Create(project);
                uow.ProjectRepository.Save();

                return RedirectToAction("Index", "Projects");
            }

            return View(project);
        }
       
        public ActionResult Confirm(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int ID = (int)id;

            Project project = uow.ProjectRepository.GetProjectByID(ID);

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = "Ready";
            ViewBag.Owner = Session["ID"];

            Session["ProjectID"] = id;

            return View("Confirm", project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                int id = (int)Session["ID"];

                var tasks = uow.TaskRepository.GetAllTaskForProject(id);

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        if (task.Status.ToString().Equals("InProgress"))
                        {
                            return RedirectToAction("List", "Tasks");
                        }
                    }
                }

                uow.ProjectRepository.Update(project);
                uow.ProjectRepository.Save();

                Session["ProjectID"] = project.ID;

                return RedirectToAction("Create", "Incomes");
            }

            ViewBag.Owner = Session["ID"];

            return View(project);
        }

        public ActionResult Edit(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int ID = (int)id;

            Project project = uow.ProjectRepository.GetProjectByID(ID);

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Owner = Session["ID"];

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeveleperID,Title,Description,Category,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                uow.ProjectRepository.Update(project);
                uow.ProjectRepository.Save();

                return RedirectToAction("Index", "Projects");
            }

            ViewBag.Owner = Session["ID"];

            return View(project);
        }

        public ActionResult Delete(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int ID = (int)id;

            Project project = uow.ProjectRepository.GetProjectByID(ID);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = uow.ProjectRepository.GetProjectByID(id);
            var tasks = uow.TaskRepository.GetAllTaskForProject(id);

            foreach(var task in tasks)
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
