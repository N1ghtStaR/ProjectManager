namespace ProjectManagerDataAccess.Repositories
{
    using System.Threading.Tasks;
    using System.Threading;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Common;

    class UnitOfWork
    {
        private ProjectManagerDbContext context;

        private Repository<ProjectManagerDB.Entities.Developer> developerRepository;
        private Repository<ProjectManagerDB.Entities.Project> projectRepository;
        private Repository<ProjectManagerDB.Entities.Task> taskRepository;
        private Repository<ProjectManagerDB.Entities.Income> incomeRepository;

        public UnitOfWork(ProjectManagerDbContext ctx)
        {
            context = ctx;
        }

        ///////////////////////////////////////////////////////////////
        //                                                           //
        //     Описва се точния път до всяко entity                  //
        //        не се ползва "using ProjectManagerDB.Entities"     //
        //        настъпва грешка поради entity<Task>                //
        //                                                           //
        //     using System.Threading.Tasks                          //
        //     using ProjectManagerDB.Entities                       //
        //                                                           //
        ///////////////////////////////////////////////////////////////


        public Repository<ProjectManagerDB.Entities.Developer> DeveloperRepository
        {
            get
            {
                if (developerRepository == null)
                {
                    developerRepository = new Repository<ProjectManagerDB.Entities.Developer>(context);
                }

                return developerRepository;
            }
        }

        public Repository<ProjectManagerDB.Entities.Project> ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                {
                    projectRepository = new Repository<ProjectManagerDB.Entities.Project>(context);
                }

                return projectRepository;
            }
        }

        public Repository<ProjectManagerDB.Entities.Income> IncomeRepository
        {
            get
            {
                if(incomeRepository == null)
                {
                    incomeRepository = new Repository<ProjectManagerDB.Entities.Income>(context);
                }

                return incomeRepository;
            }
        }

        public Repository<ProjectManagerDB.Entities.Task> TaskRepository
        {
            get
            {
                if (taskRepository == null)
                {
                    taskRepository = new Repository<ProjectManagerDB.Entities.Task>(context);
                }

                return taskRepository;
            }
        }

        public DbTransaction Transaction { get; private set; }

        public virtual int SaveChanges() => context.SaveChanges();

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            return await context.Database.ExecuteSqlCommandAsync(sql, cancellationToken, parameters);
        }

        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            if (objectContext.Connection.State != ConnectionState.Open)
            {
                objectContext.Connection.Open();
            }

            Transaction = objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public virtual bool Commit()
        {
            Transaction.Commit();

            return true;
        }

        public virtual void Rollback()
        {
            Transaction.Rollback();
        }

    }
}
