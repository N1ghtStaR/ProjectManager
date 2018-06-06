namespace ProjectManagerFactory.Factories.ProjectFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public interface IProjectFactory
    {
        Project New(ProjectViewModel viewModel);
    }
}
