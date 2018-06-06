namespace ProjectManagerFactory.Factories.TaskFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public interface ITaskFactory
    {
        Task New(TaskViewModel viewModel);
    }
}
