namespace ProjectManagerFactory.Factories.IncomeFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public interface IIncomeFactory
    {
        Income New(IncomeViewModel viewModel);
    }
}
