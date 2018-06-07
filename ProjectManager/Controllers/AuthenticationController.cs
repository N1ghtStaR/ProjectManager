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
    using ProjectManagerFactory;

    public class AuthenticationController : Controller
    {
        private readonly UnitOfWork uow;
        private readonly Factory factory; 

        public AuthenticationController()
        {
            uow = new UnitOfWork(new ProjectManagerDbContext());
            factory = new Factory();
        }

        public AuthenticationController(ProjectManagerDbContext context)
        {
            uow = new UnitOfWork(context);
        }

        [IsAuthenticated]
        public ActionResult DeleteMessage(int id)
        {
            if(Session["Role"].ToString().Equals("TeamLeader"))
            {
                uow.MessageRepository.DeleteMessage(id);
                uow.MessageRepository.Save();

                return RedirectToAction("MessageRoom", "Authentication");
            }

            return RedirectToAction("Index", "Home");
        }

        [IsAuthenticated]
        [HttpGet]
        public ActionResult MessageRoom()
        {
            IEnumerable<Message> messages = uow.MessageRepository.GetMessages();

            List<MessageViewModel> chat = new List<MessageViewModel>();

            foreach (Message message in messages)
            {
                MessageViewModel model = new MessageViewModel(message);
                chat.Add(model);
            }

            return View(chat);
        }

        [IsAuthenticated]
        [HttpPost]
        public ActionResult Chat(int developerID, string username, string description, DateTime date)
        {
            int id = 0;

            IEnumerable<Message> check = uow.MessageRepository.GetMessages();

            if(check == null)
            {
                id = 1;
            }
            else
            {
                List<Message> messages = new List<Message>();

                foreach(Message m in check)
                {
                    if(m.ID > id)
                    {
                        id = m.ID;
                    }
                }

                id += 1;
            }

            MessageViewModel ms = new MessageViewModel
            {
                ID = id,
                DeveloperID = developerID,
                Username = username,
                Description = description,
                Date = date
            };

            Message message = factory.MessageFactory.New(ms);

            uow.MessageRepository.New(message);
            uow.MessageRepository.Save();

            return RedirectToAction("MessageRoom", "Authentication");
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
                    try                                                           
                    {                                                             
                        Developer developer = factory.DeveloperFactory.New(developerModel);                                                 
                                                                                  
                        uow.DeveloperRepository.Registration(developer);          
                        uow.DeveloperRepository.Save();                           
                    }                                                             
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
                    Developer developer = factory.DeveloperFactory.New(developerModel);

                    uow.DeveloperRepository.PromoteOrDemote(developer);
                    uow.DeveloperRepository.Save();
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later!");

                    return View("AccountDetails", developerModel);
                }

                TempData["UpdatedDeveloper"] = developerModel.Username + "'s role have been updated!";

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
        public ActionResult AccountDetails(int? id)                                             
        {                                                                                       
            if (id == null)                                                                     
            {                                                                                   
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                     
            }                                                                                   
                                                                                                
            Developer developer = uow.DeveloperRepository.GetDeveloperByID((int)id);            
                                                                                                
            if (developer == null)                                                              
            {                                                                                   
                return HttpNotFound();                                                          
            }                                                                                   
                                                                                                
            DeveloperViewModel developerModel = new DeveloperViewModel(developer);              
                                                                                                
            if (id == (int)Session["ID"])                                                       
            {                                                                                   
                IEnumerable<Income> incomes = uow.IncomeRepository.GetIncomesForUser((int)id);  
                                                                                                
                double incomeAmount = 0;                                                        
                                                                                                
                if (incomes != null)                                                            
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
