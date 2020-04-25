using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// Provides generic base repository functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool Exists(Expression<Func<T, bool>> expression);
    }
}