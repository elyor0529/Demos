using System;
using System.Linq;
using System.Linq.Expressions;

namespace Task2.Common.BLL.Service
{
    public interface IEntityService<T>
    {

        void Create(T entity);

        void Delete(T entity, bool directly);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);

        T GetById(long id);

        void Update(T entity);
    }
}
