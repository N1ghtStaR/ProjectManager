namespace ProjectManagerDB
{
    using ProjectManagerDB.Entities;
    using System.Data.Entity;

    public partial class ProjectManagerDbContext : DbContext
    {
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ProjectManagerDbContext() : base("name=ProjectManagerDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
