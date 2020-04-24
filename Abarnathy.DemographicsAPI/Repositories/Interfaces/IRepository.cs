using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// Provides generic base repository functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool Exists(Expression<Func<T, bool>> expression);
    }
}