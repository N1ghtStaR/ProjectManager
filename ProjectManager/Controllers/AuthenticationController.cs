namespace ProjectManager.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using ProjectManagerDataAccess;
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;

    public class AuthenticationController : Controller
    {
        private readonly UnitOfWork uow;

        public AuthenticationController()
        {
            this.uow = new UnitOfWork(new ProjectManagerDbContext());
        }
        public AuthenticationController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        public ActionResult Registration()
        {
            if(Session["ID"] == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "ID,Username,Email,Password,Role")] Developer developer)
        {
            if(Session["ID"] == null)
            {
                if (ModelState.IsValid)
                {
                    uow.DeveloperRepository.Registration(developer);
                    uow.DeveloperRepository.Save();

                    Session["ID"] = developer.ID;
                    Session["Username"] = developer.Username;
                    Session["Role"] = developer.Role;

                    Session["ProjectID"] = null;
                    Session["ProjectTitle"] = null;
                    Session["ProjectStatus"] = null;

                    return RedirectToAction("Index", "Home");
                }

                return View(developer);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogIn()
        {
            if(Session["ID"] == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Developer developer = uow.DeveloperRepository.LogIn(username, password);

                    Session["ID"] = developer.ID;
                    Session["Username"] = developer.Username;
                    Session["Role"] = developer.Role;

                    Session["ProjectID"] = null;
                    Session["ProjectTitle"] = null;
                    Session["ProjectStatus"] = null;

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,Username,Password,Email,Role")] Developer developer)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            uow.DeveloperRepository.PromoteOrDemote(developer);
            uow.DeveloperRepository.Save();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            if(Session["ID"] != null)
            {
                Session["ID"] = null;
                Session["Username"] = null;
                Session["Role"] = null;

                Session["ProjectID"] = null;
                Session["ProjectTitle"] = null;
                Session["ProjectStatus"] = null;
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccountDetails(int? id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int ID = (int)id;

            Developer developer = uow.DeveloperRepository.GetDeveloperByID(ID);

            if (developer == null)
            {
                return HttpNotFound();
            }

            if (developer.Role.ToString().Equals("Developer"))
            {
                ViewBag.SubmitValue = "Promote";
                ViewBag.RoleValue = "TeamLeader";
            }
            else
            {
                ViewBag.SubmitValue = "Demote";
                ViewBag.RoleValue = "Developer";
            }

            return View(developer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.DeveloperRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
