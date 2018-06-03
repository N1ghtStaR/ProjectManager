namespace ProjectManager.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using ProjectManager.Filters;
    using ProjectManager.Models;
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

        [Authenticated]
        public ActionResult Registration()
        {
            return View();
        }

        [Authenticated]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "ID,Username,Email,Password,Role")] DeveloperViewModel developerModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Developer registered = uow.DeveloperRepository.IsRegistered(developerModel.Username); 
                                                                                                          
                    ModelState.AddModelError("", "User with that username is already registered!");       
                                                                                                          
                    return View(developerModel);                                                          
                }
                catch
                {
                    try                                                           ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    {                                                             //                                                                                                                   //
                        Developer developer = new Developer                       //   1. Проверка за валидността на DeveloperViewModel                                                                //
                        {                                                         //   -------------------------------------------------------------------------------------------------------------   //
                            ID = developerModel.ID,                               //   2. Проверка дали вече съществува регистриран потребител със сответния username                                  //
                            Username = developerModel.Username,                   //      2.1 False -> Връща грещка, че вече съществува потребител със съответния username                             //
                            Email = developerModel.Email,                         //      2.2 True -> Exception -> Инстанцира се нов обект от клас Developer присвояващ данните на developerModel      //
                            Password = developerModel.Password,                   //   -------------------------------------------------------------------------------------------------------------   //
                            Role = (Developer.Character)developerModel.Role       //      2.3 Проверка за грешки при добавянето на новия обект в базата                                                //
                        };                                                        //         2.3.1 True -> Exception -> Връща грешка в View()-то                                                       //
                                                                                  //         2.3.2 False -> Създава нов запис в базата                                                                 //
                        uow.DeveloperRepository.Registration(developer);          //                                                                                                                   //
                        uow.DeveloperRepository.Save();                           //   !!! Трябва да хеширам паролите -> сега ме мързи -> ДА ГО НАПРАВЯ УТРЕ !!!                                       //
                    }                                                             ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    catch
                    {
                        ModelState.AddModelError("", "There was a problem creating your account!");

                        return View(developerModel);
                    }

                    Session["ID"] = developerModel.ID;
                    Session["Username"] = developerModel.Username;
                    Session["Role"] = developerModel.Role;

                    Session["ProjectID"] = null;
                    Session["ProjectTitle"] = null;
                    Session["ProjectStatus"] = null;

                    return RedirectToAction("Index", "Home");
                }
                
            }

            return View(developerModel);
        }

        [Authenticated]
        public ActionResult LogIn()
        {
            return View();
        }

        [Authenticated]
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
                    ModelState.AddModelError("", "Username or password are incorrect!");
                }
            }

            return View();
        }

        [IsAuthenticated]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(DeveloperViewModel developerModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Developer developer = new Developer()
                    {
                        ID = developerModel.ID,
                        Username = developerModel.Username,
                        Email = developerModel.Email,
                        Password = developerModel.Password,
                        Role = (Developer.Character)developerModel.Role
                    };

                    uow.DeveloperRepository.PromoteOrDemote(developer);
                    uow.DeveloperRepository.Save();
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later!");

                    return View("AccountDetails", developerModel);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(developerModel);
        }

        [IsAuthenticated]
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

        [IsAuthenticated]
        public ActionResult AccountDetails(int? id)                                             //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {                                                                                       //                                                                                                              //
            if (id == null)                                                                     //                                          Account Details                                                     //
            {                                                                                   //                                                                                                              //
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                     //   1. Взима потребител от базата данни по съответно ID -> присвоява се на обект от клас DeveloperViewModel    //
            }                                                                                   //   --------------------------------------------------------------------------------------------------------   //
                                                                                                //   2. Ако developerModel.ID е равно  на Session["ID"] -> взима цялостна информация за съответния потребител   //
            Developer developer = uow.DeveloperRepository.GetDeveloperByID((int)id);            //   --------------------------------------------------------------------------------------------------------   //
                                                                                                //   3. Приходи                                                                                                 //
            if (developer == null)                                                              //      3.1 Общ приход                                                                                          //
            {                                                                                   //      3.2 ДДС ( 20% от общ приход )          |   Показват се в случай, че "Общ приход" е                      //
                return HttpNotFound();                                                          //      3.3 Чист приход ( Общ приход - ДДС )   |   различен от 0                                                //
            }                                                                                   //   --------------------------------------------------------------------------------------------------------   //
                                                                                                //   4. Проекти ( Администратора има достъп до тази информация на всеки потребител )                            //
            DeveloperViewModel developerModel = new DeveloperViewModel(developer);              //      4.1 Брой проекти                                                                                        //
                                                                                                //      4.2 Брой Завършени проекти     |   Показват се в случай, че "Брой проекти" е                            //
            if (id == (int)Session["ID"])                                                       //      4.3 Брой Незавършени проекти   |   различен от 0                                                        //
            {                                                                                   //   --------------------------------------------------------------------------------------------------------   //
                IEnumerable<Income> incomes = uow.IncomeRepository.GetIncomesForUser((int)id);  //   5. Администраторски възможности                                                                            //
                                                                                                //      5.1 User Promote ( Developer -> TeamLeader )   |    Възможно е в случай, че ID-то на потребителския е   //
                double incomeAmount = 0;                                                        //      5.2 User Demote ( TeamLeader -> Developer )    |    различно от това на Session["ID"]                   //
                                                                                                //                                                                                                              //
                if (incomes != null)                                                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                {
                    List<IncomeViewModel> incomesModel = new List<IncomeViewModel>();

                    foreach (Income i in incomes)
                    {
                        IncomeViewModel incomeModel = new IncomeViewModel(i);
                        incomesModel.Add(incomeModel);
                    }

                    foreach(IncomeViewModel income in incomesModel)
                    {
                        incomeAmount += income.Amount;
                    }

                    double dds = (incomeAmount * 20) / 100;
                    double cash = incomeAmount - dds;

                    ViewBag.DDS = dds;
                    ViewBag.Cash = cash;
                }

                ViewBag.TotalIncomes = incomeAmount;
            }

            IEnumerable<Project> projects = uow.ProjectRepository.GetAllProjectsForUser((int)id);

            int projectsCount = 0;

            if (projects != null)
            {
                List<ProjectViewModel> projectsModel = new List<ProjectViewModel>();

                foreach(Project project in projects)
                {
                    ProjectViewModel projectModel = new ProjectViewModel(project);
                    projectsModel.Add(projectModel);
                }

                foreach (ProjectViewModel project in projectsModel)
                {
                    projectsCount++;
                }

                List<ProjectViewModel> projectsInProgressModel = new List<ProjectViewModel>();
                List<ProjectViewModel> projectsReadyModel = new List<ProjectViewModel>();

                foreach(Project project in projects)
                {
                    if(project.Status.ToString().Equals("Ready"))
                    {
                        ProjectViewModel projectInProgress = new ProjectViewModel(project);
                        projectsInProgressModel.Add(projectInProgress);
                    }
                    else
                    {
                        ProjectViewModel projectReady = new ProjectViewModel(project);
                        projectsReadyModel.Add(projectReady);
                    }
                }

                int inProgress = 0;
                int ready = 0;

                if (projectsReadyModel != null)
                {
                    foreach (ProjectViewModel inProgressProject in projectsReadyModel)
                    {
                        inProgress++;
                    }

                    ViewBag.ProjectsInProgress = inProgress;
                }

                if (projectsReadyModel != null)
                {
                    foreach (ProjectViewModel readyProject in projectsReadyModel)
                    {
                        ready++;
                    }

                    ViewBag.ProjectsReady = ready;
                }

                ViewBag.Projects = projectsCount;
                ViewBag.ProjectsCount = projectsCount;
            }

            if (developerModel.Role.ToString().Equals("Developer"))
            {
                ViewBag.SubmitValue = "Promote";
                ViewBag.RoleValue = "TeamLeader";
            }
            else
            {
                ViewBag.SubmitValue = "Demote";
                ViewBag.RoleValue = "Developer";
            }

            return View(developerModel);
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
