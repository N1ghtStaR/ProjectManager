namespace ProjectManagerDataAccess.Repositories.ProjectRepository
{
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ProjectRepository : IProjectRepository, IDisposable
    {
        private readonly ProjectManagerDbContext Context;
        private bool disposed = false;

        public ProjectRepository(ProjectManagerDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<Project> GetAllProjectsForUser(int id)
        {
            return Context.Projects.Where(p => p.DeveleperID.Equals(id)).ToList();
        }

        public IEnumerable<Project> GetProjectsByTitle(string title, int id)
        {
            return Context.Projects.Where(p => p.Title.Contains(title) && p.DeveleperID.Equals(id)).ToList();
        }

        public IEnumerable<Project> GetProjectsByStatus(string status, int id)
        {
            return Context.Projects.Where(p => p.Status.ToString().Equals(status) && p.DeveleperID.Equals(id)).ToList();
        }

        public Project GetProjectByID(int id)
        {
            return Context.Projects.Find(id);
        }

        public void Create(Project project)
        {
            Context.Projects.Add(project);
        }

        public void Update(Project project)
        {
            Context.Entry(project).State = EntityState.Modified;
        }
        public void Delete(Project project)
        {
            Context.Projects.Remove(project);
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
