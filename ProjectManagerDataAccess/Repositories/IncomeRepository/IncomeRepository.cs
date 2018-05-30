namespace ProjectManagerDataAccess.Repositories.IncomeRepository
{
    using ProjectManagerDB;
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IncomeRepository : IIncomeRepository , IDisposable
    {
        private readonly ProjectManagerDbContext Context;
        private bool disposed = false;

        public IncomeRepository(ProjectManagerDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<Income> GetIncomesForUser(int id)
        {
            return Context.Incomes.Where(i => i.DeveleperID.Equals(id)).ToList();
        }

        public Income GetIncomeByID(int id)
        {
            return Context.Incomes.Find(id);
        }

        public void Create(Income income)
        {
            Context.Incomes.Add(income);
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
