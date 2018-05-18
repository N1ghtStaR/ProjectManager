namespace ProjectManagerServices
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IService<TEntity> where TEntity : class
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);

        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Delete(int id);
        void Delete(TEntity entity);
    }
}
