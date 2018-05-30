namespace ProjectManagerDataAccess
{
    using ProjectManagerDataAccess.Repositories.DeveloperRepository;
    using ProjectManagerDataAccess.Repositories.IncomeRepository;
    using ProjectManagerDataAccess.Repositories.ProjectRepository;
    using ProjectManagerDataAccess.Repositories.TaskRepository;
    using ProjectManagerDB;

    public class UnitOfWork
    {
        private readonly ProjectManagerDbContext context;

        private DeveloperRepository developerRepository;
        private ProjectRepository projectRepository;
        private TaskRepository taskRepository;
        private IncomeRepository incomeRepository;

        public UnitOfWork(ProjectManagerDbContext connection)
        {
            context = connection;
        }

        public DeveloperRepository DeveloperRepository
        {
            get
            {
                if(developerRepository == null)
                {
                    developerRepository = new DeveloperRepository(context);
                }

                return developerRepository;
            }
        }

        public ProjectRepository ProjectRepository
        {
            get
            {
                if(projectRepository == null)
                {
                    projectRepository = new ProjectRepository(context);
                }

                return projectRepository;
            }
        }

        public TaskRepository TaskRepository
        {
            get
            {
                if(taskRepository == null)
                {
                    taskRepository = new TaskRepository(context);
                }

                return taskRepository;
            }
        }

        public IncomeRepository IncomeRepository
        {
            get
            {
                if(incomeRepository == null)
                {
                    incomeRepository = new IncomeRepository(context);
                }

                return incomeRepository;
            }
        }

    }
}
