namespace ProjectManager.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDataAccess.Repositories.ProjectRepository;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;

    public class ProjectsController : Controller
    {
        private IProjectRepository projectRepository;

        public ProjectsController()
        {
            this.projectRepository = new ProjectRepository(new ProjectManagerDbContext());
        }

        public ProjectsController(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
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
                return View(projectRepository.GetProjectsByTitle(projectTitle, id));
            }

            return View(projectRepository.GetAllProjectsForUser(id));
        }
        
        public ActionResult Status(string status)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("LogIn", "Authentication");
            }

            int id = (int)Session["ID"];

            return View("Index", projectRepository.GetProjectsByStatus(status, id));
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
                projectRepository.Create(project);
                projectRepository.Save();

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
            Project project = projectRepository.GetProjectByID(ID);

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = "Ready";
            ViewBag.Owner = Session["ID"];

            Session["ProjectID"] = id;
            

            return View("Confirm", project);
        }
/*
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
                var tasks = db.Tasks
                                .Where(p => p.ProjectID.Equals(id));

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

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                Session["ProjectID"] = project.ID;

                return RedirectToAction("Create", "Incomes");
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", project.DeveleperID);

            return View(project);
        }
*/
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
            Project project = projectRepository.GetProjectByID(ID);

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
                projectRepository.Update(project);
                projectRepository.Save();

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
            Project project = projectRepository.GetProjectByID(ID);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }
/*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = projectRepository.GetProjectByID(id);
            projectRepository.Delete(project);
            projectRepository.Save();

            return RedirectToAction("Index", "Projects");
        }
*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
