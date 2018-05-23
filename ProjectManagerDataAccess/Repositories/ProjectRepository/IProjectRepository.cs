namespace ProjectManagerDataAccess.Repositories.ProjectRepository
{
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;


    public interface IProjectRepository : IDisposable
    {
        IEnumerable<Project> GetAllProjectsForUser(int ID);
        IEnumerable<Project> GetProjectsByTitle(string title, int id);
        IEnumerable<Project> GetProjectsByStatus(string status, int id);

        Project GetProjectByID(int projectID);

        void Create(Project project);
        void Update(Project project);
        void Delete(Project project);
        void Save();
    }
}
