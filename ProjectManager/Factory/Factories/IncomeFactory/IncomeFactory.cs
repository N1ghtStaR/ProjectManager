namespace ProjectManagerFactory.Factories.IncomeFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public class IncomeFactory : IIncomeFactory
    {
        public Income New(IncomeViewModel viewModel)
        {
            Income income = new Income
            {
                ProjectID = viewModel.ProjectID,
                DeveleperID = viewModel.DeveleperID,
                Title = viewModel.Title,
                Amount = viewModel.Amount,
                ReleaseDate = viewModel.ReleaseDate
            };

            return income;
        }
    }
}
