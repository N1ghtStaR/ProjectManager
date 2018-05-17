using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ProjectManagerDB;
using ProjectManagerDB.Entities;

namespace ProjectManager.Controllers
{
    public class IncomesController : Controller
    {
        private ProjectManagerDbContext db = new ProjectManagerDbContext();

        public ActionResult FindIncomes(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            Session["ProjectID"] = id;
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            int id = (int)Session["ID"];
            var incomes = db.Incomes
                        .Include(i => i.Develeper)
                        .Include(i => i.Project)
                        .Where(i => i.DeveleperID.Equals(id));
            return View(incomes.ToList());
        }

        public ActionResult Create()
        {
            int id = (int)Session["ProjectID"];

            Project project = db.Projects.Find(id);

            ViewBag.ProjectName = project.Title;

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", Session["ID"]);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", Session["ProjectID"]);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,DeveleperID,Title,Amount,ReleaseDate")] Income income)
        {
            if (ModelState.IsValid)
            {
                db.Incomes.Add(income);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeveleperID = new SelectList(db.Developers, "ID", "Username", income.DeveleperID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Title", income.ProjectID);
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

            Income income = db.Incomes.Find(id);

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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
