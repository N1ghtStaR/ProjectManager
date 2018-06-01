using ProjectManagerDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerDataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ProjectManagerDbContext context;
        protected readonly DbSet<TEntity> Set;

        public BaseRepository(ProjectManagerDbContext context)
        {
            this.context = context;
            Set = context.Set<TEntity>();
        }

        public TEntity GetByID(int id)
        {
            return Set.Find(id);
        }

        public void Create(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Set.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }
    }
}
