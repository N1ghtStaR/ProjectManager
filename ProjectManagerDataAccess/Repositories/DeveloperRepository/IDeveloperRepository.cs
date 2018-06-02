namespace ProjectManagerDataAccess.Repositories.DeveloperRepository
{
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;

    public interface IDeveloperRepository : IDisposable
    {
        IEnumerable<Developer> GetAllDevelopers(int id);
        IEnumerable<Developer> GetDevelopersByUsername(string username);

        Developer LogIn(string username, string password);
        Developer IsRegistered(string username);
        Developer GetDeveloperByID(int developerID);

        void Registration(Developer developer);
        void PromoteOrDemote(Developer developer);

        void Save();
    }
}
