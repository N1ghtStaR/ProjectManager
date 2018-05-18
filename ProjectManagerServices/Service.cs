namespace ProjectManagerServices
{
    using ProjectManagerDataAccess.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private IRepository<TEntity> repository;

        protected Service(IRepository<TEntity> repo)
        {
            repository = repo;
        }


        public void Delete(int id)
        {
            var entity = repository.Get(id);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            repository.Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            repository.AddRange(entities);
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
