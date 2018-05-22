namespace ProjectManagerDataAccess.Repositories.DeveloperRepository
{
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;

    public interface IDeveloperRepository : IDisposable
    {
        IEnumerable<Developer> GetDevelopers();
        IEnumerable<Developer> GetDevelopersByUsername(string username);

        Developer LogIn(string username, string password);
        Developer GetDeveloperByID(int developerID);

        void InsertDeveloper(Developer developer);
        void UpdateDeveloper(Developer developer);

        void Save();
    }
}
