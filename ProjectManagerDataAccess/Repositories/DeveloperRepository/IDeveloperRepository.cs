namespace ProjectManagerDataAccess.Repositories.DeveloperRepository
{
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;

    public interface IDeveloperRepository : IDisposable
    {
        IEnumerable<Developer> GetAllDevelopers();
        IEnumerable<Developer> GetDevelopersByUsername(string username);

        Developer LogIn(string username, string password);
        Developer GetDeveloperByID(int developerID);

        void Registration(Developer developer);
        void PromoteOrDemote(Developer developer);

        void Save();
    }
}
