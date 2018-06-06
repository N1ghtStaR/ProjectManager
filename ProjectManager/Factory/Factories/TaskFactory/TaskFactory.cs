namespace ProjectManagerFactory.Factories.TaskFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public class TaskFactory : ITaskFactory
    {
        public Task New(TaskViewModel viewModel)
        {
            Task task = new Task
            {
                ID = viewModel.ID,
                ProjectID = viewModel.ProjectID,
                Description = viewModel.Description,
                Priority = (Task.Anteriority)viewModel.Priority,
                Status = (Task.Stats)viewModel.Status
            };

            return task;
        }
    }
}
