using System;
using System.Linq;
using System.Linq.Expressions;
using PagedList;
using Task2.Common.DAL.EF;

namespace Task2.Common.BLL.Repository
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : IComparable
    {

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        IPagedList<TEntity> Paginate(int pageIndex, int pageSize);

        TEntity GetSingle(TId id);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, long>
        where TEntity : class, IEntity<long>
    {
    }

    /// <summary>
    /// Entity Framework interface implementation for IRepository.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which implements IEntity of int</typeparam>
    public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, long>
        where TEntity : class, IEntity<long>
    {

    }


    /// <summary>
    /// Entity Framework interface implementation for IRepository.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TId">Type of entity Id</typeparam>
    public interface IEntityRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : IComparable
    {

        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetSingleIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties);

        IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector);

        IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector);

        IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        void Add(TEntity entity);

        void AddGraph(TEntity entity);

        void Edit(TEntity entity);

        void EditGraph(TEntity entity);

        void AddOrEdit(params TEntity[] entities);

        void Delete(TEntity entity, bool directly = false);

        int Save();

    }
}