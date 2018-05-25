namespace ProjectManagerDataAccess.Repositories.TaskRepository
{
    using ProjectManagerDB.Entities;
    using System;

    using System.Collections.Generic;
    public interface ITaskRepository : IDisposable
    {
        IEnumerable<Task> GetAllTaskForProject(int id);
        IEnumerable<Task> GetTasksByStatus(int id, string status);

        Task GetTaskByID(int id);

        void Create(Task task);
        void Update(Task task);
        void Delete(Task task);
        void Save();
    }
}
