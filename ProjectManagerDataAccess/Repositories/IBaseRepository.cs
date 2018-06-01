using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerDataAccess.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity GetByID(int id);

        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
