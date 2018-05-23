namespace ProjectManagerDataAccess.Repositories.IncomeRepository
{
    using ProjectManagerDB.Entities;
    using System;
    using System.Collections.Generic;

    public interface IIncomeRepository : IDisposable
    {
        IEnumerable<Income> GetIncomesForUser(int id);

        Income GetIncomeByID(int id);

        void Create(Income income);
        void Save();
    }
}
