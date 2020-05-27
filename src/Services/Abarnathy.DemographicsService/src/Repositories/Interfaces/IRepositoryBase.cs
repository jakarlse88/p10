using System;
using System.Linq;
using System.Linq.Expressions;

namespace Abarnathy.DemographicsService.Repositories
{
    /// <summary>
    /// Provides generic base repository functionality.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}