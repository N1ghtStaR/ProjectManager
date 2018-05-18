namespace ProjectManagerDataAccess
{
    using System.Data.Entity;

    public class ProjectManagerDbContext : DbContext
    {
        public DbSet<ProjectManagerDB.Entities.Developer> Developers { get; set; }
        public DbSet<ProjectManagerDB.Entities.Project> Projects { get; set; }
        public DbSet<ProjectManagerDB.Entities.Task> Tasks { get; set; }
        public DbSet<ProjectManagerDB.Entities.Income> Incomes { get; set; }
    }
}
