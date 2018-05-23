namespace ProjectManagerDataAccess.Repositories.TaskRepository
{
    using ProjectManagerDB.Entities;
    using System;

    using System.Collections.Generic;
    interface ITaskRepository : IDisposable
    {
        IEnumerable<Task> GetAllTaskForProject(int id);
        IEnumerable<Task> GetTasksByStatus(string status);

        Task GetTaskByID(int id);

        void Create(Task task);
        void Update(Task task);
        void Save();
    }
}
