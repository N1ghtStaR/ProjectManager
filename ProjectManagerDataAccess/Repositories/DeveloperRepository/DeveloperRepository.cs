using ProjectManagerDB;
using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerDataAccess.Repositories.DeveloperRepository
{
    public class DeveloperRepository : IDeveloperRepository , IDisposable
    {
        private readonly ProjectManagerDbContext Context;
        private bool disposed = false;

        public DeveloperRepository(ProjectManagerDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<Developer> GetAllDevelopers()
        {
            return Context.Developers
                                .ToList();
        }

        public IEnumerable<Developer> GetDevelopersByUsername(string username)
        {
            return Context.Developers
                                .Where(d => d.Username.Contains(username))
                                .ToList();
        }

        public Developer GetDeveloperByID(int id)
        {
            return Context.Developers
                                .Find(id);
        }

        public Developer LogIn(string username, string password)
        {
            return Context.Developers
                                .Where(d => d.Username.Equals(username) && d.Password.Equals(password))
                                .Single();
        }

        public void Registration(Developer developer)
        {
            Context.Developers
                        .Add(developer);
        }


        public void PromoteOrDemote(Developer developer)
        {
            Context.Entry(developer).State = EntityState.Modified;
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
