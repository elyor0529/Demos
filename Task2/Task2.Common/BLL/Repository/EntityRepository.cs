using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PagedList;
using Task2.Common.DAL.EF;
using Task2.Common.Enums;

namespace Task2.Common.BLL.Repository
{

    /// <summary>
    /// IEntityRepository implementation for DbContext instance.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TId">Type of entity Id</typeparam>
    public abstract class EntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : IComparable
    {

        private readonly DbContext _dbContext;

        protected EntityRepository(DbContext dbContext)
        {

            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(w => w.IsDeleted != true);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetAll();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            var queryable = GetAll().Where<TEntity>(predicate);

            return queryable;
        }

        public IPagedList<TEntity> Paginate(int pageIndex, int pageSize)
        {
            var paginatedList = Paginate<TId>(pageIndex, pageSize, x => x.Id);

            return paginatedList;
        }

        public IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector)
        {
            return Paginate<TKey>(pageIndex, pageSize, keySelector, null);
        }

        public IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var paginatedList = Paginate<TKey>(pageIndex, pageSize, keySelector, predicate, OrderByType.Ascending, includeProperties);

            return paginatedList;
        }

        public IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector)
        {
            return PaginateDescending<TKey>(pageIndex, pageSize, keySelector, null);
        }

        public IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var paginatedList = Paginate<TKey>(pageIndex, pageSize, keySelector, predicate, OrderByType.Descending, includeProperties);

            return paginatedList;
        }

        public TEntity GetSingle(TId id)
        {
            var entities = GetAll();
            var entity = Filter<TId>(entities, x => x.Id, id).FirstOrDefault();

            return entity;
        }

        public TEntity GetSingleIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = GetAllIncluding(includeProperties);
            var entity = Filter<TId>(entities, x => x.Id, id).FirstOrDefault();

            return entity;
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public void AddGraph(TEntity entity)
        {
            //TODO:use GraphDiff
        }

        public void Edit(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity, bool directly = false)
        {
            if (directly)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                entity.IsDeleted = true;
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        private IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderByType orderByType, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> queryable =
                (orderByType == OrderByType.Ascending)
                    ? GetAllIncluding(includeProperties).OrderBy(keySelector)
                    : GetAllIncluding(includeProperties).OrderByDescending(keySelector);

            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;

            var paginatedList = queryable.ToPagedList(pageIndex, pageSize);

            return paginatedList;
        }

        private IQueryable<TEntity> Filter<TProperty>(IQueryable<TEntity> dbSet,
            Expression<Func<TEntity, TProperty>> property, TProperty value)
            where TProperty : IComparable
        {

            var memberExpression = property.Body as MemberExpression;

            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
            {
                throw new ArgumentException("Property expected", "property");
            }

            var left = property.Body;
            var right = Expression.Constant(value, typeof(TProperty));
            var searchExpression = Expression.Equal(left, right);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, new ParameterExpression[] { property.Parameters.Single() });

            return dbSet.Where(lambda);
        }

        public virtual void EditGraph(TEntity entity)
        {
            //TODO:use GraphDiff

        }

        public IQueryable<TEntity> GetIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetAll();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }
            var entity = Filter<TId>(queryable, x => x.Id, id);

            return entity;
        }

        public virtual void AddOrEdit(params TEntity[] entities)
        {
        }
    }

    /// <summary>
    /// IEntityRepository implementation for DbContext instance where the TId type is Int32.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class EntityRepository<TEntity> : EntityRepository<TEntity, long>, IEntityRepository<TEntity>
        where TEntity : class, IEntity<long>
    {

        public EntityRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}