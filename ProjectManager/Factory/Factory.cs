namespace ProjectManagerFactory
{
    using ProjectManagerFactory.Factories.DeveloperFactory;
    using ProjectManagerFactory.Factories.IncomeFactory;
    using ProjectManagerFactory.Factories.ProjectFactory;
    using ProjectManagerFactory.Factories.TaskFactory;

    class Factory
    {
        private DeveloperFactory developerFactory;
        private ProjectFactory projectFactory;
        private TaskFactory taskFactory;
        private IncomeFactory incomeFactory;

        public DeveloperFactory DeveloperFactory
        {
            get
            {
                if(developerFactory == null)
                {
                    developerFactory = new DeveloperFactory();
                }

                return developerFactory;
            }
        }

        public ProjectFactory ProjectFactory
        {
            get
            {
                if(projectFactory == null)
                {
                    projectFactory = new ProjectFactory();
                }

                return projectFactory;
            }
        }

        public TaskFactory TaskFactory
        {
            get
            {
                if(taskFactory == null)
                {
                    taskFactory = new TaskFactory();
                }

                return taskFactory;
            }
        }

        public IncomeFactory IncomeFactory
        {
            get
            {
                if(incomeFactory == null)
                {
                    incomeFactory = new IncomeFactory();
                }

                return incomeFactory;
            }
        }
    }
}
