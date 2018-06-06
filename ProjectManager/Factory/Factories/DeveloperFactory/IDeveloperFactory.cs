namespace ProjectManagerFactory.Factories.DeveloperFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public interface IDeveloperFactory
    {
        Developer New(DeveloperViewModel viewModel);
    }
}
