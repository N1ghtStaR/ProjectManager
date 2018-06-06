namespace ProjectManagerFactory.Factories.DeveloperFactory
{
    using ProjectManager.Models;
    using ProjectManagerDB.Entities;

    public class DeveloperFactory : IDeveloperFactory
    {
        public Developer New(DeveloperViewModel viewModel)
        {
            Developer developer = new Developer
            {
                ID = viewModel.ID,
                Username = viewModel.Username,
                Email = viewModel.Email,
                Password = viewModel.Password,
                Role = (Developer.Character)viewModel.Role
            };

            return developer;
        }
    }
}
