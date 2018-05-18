namespace ProjectManagerDataAccess.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

     public class MyConfiguration : DbConfiguration
     {
        public MyConfiguration()
        {
            SetDefaultConnectionFactory(new LocalDbConnectionFactory("MSSQLLocalDB"));
        }
     }
}