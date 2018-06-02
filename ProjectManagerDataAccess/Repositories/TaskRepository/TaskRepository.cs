namespace ProjectManagerDataAccess.Repositories.TaskRepository
{
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TaskRepository : ITaskRepository , IDisposable
    {
        private readonly ProjectManagerDbContext Context;
        private bool disposed = false;

        public TaskRepository(ProjectManagerDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<Task> GetAllTaskForProject(int id)
        {
            return Context.Tasks
                            .Where(t => t.ProjectID.Equals(id))
                            .OrderByDescending(i => i.ID)
                            .ToList();
        }

        public IEnumerable<Task> GetTasksByStatus(int id, string status)
        {
            return Context.Tasks
                            .Where(t => t.Status.ToString().Equals(status) && t.ProjectID.Equals(id))
                            .OrderByDescending(i => i.ID)
                            .ToList();
        }

        public Task GetTaskByID(int id)
        {
            return Context.Tasks
                            .Find(id);
        }

        public void Create(Task task)
        {
            Context.Tasks
                    .Add(task);
        }

        public void Update(Task task)
        {
            Context.Entry(task).State = EntityState.Modified;
        }

        public void Delete(Task task)
        {
            Context.Tasks
                    .Remove(task);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
