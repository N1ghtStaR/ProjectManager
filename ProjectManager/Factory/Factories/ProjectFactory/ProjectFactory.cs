namespace ProjectManagerFactory.Factories.ProjectFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public class ProjectFactory : IProjectFactory
    {
        public Project New(ProjectViewModel viewModel)
        {
            Project project = new Project
            {
                ID = viewModel.ID,
                DeveleperID = viewModel.DeveleperID,
                Title = viewModel.Title,
                Description = viewModel.Description,
                Category = (Project.Division)viewModel.Category,
                Status = (Project.Statistic)viewModel.Status
            };

            return project;
        }
    }
}
