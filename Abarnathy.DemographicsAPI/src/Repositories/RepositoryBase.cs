using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DemographicsDbContext _context;

        public RepositoryBase(DemographicsDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> predicate)
        {
            var result = _context
                .Set<T>()
                .Where(predicate)
                .AsNoTracking();

            return result;
        }

        public void Insert(T entity) 
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            
            _context
                .Set<T>()
                .Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            
            _context
                .Set<T>()
                .Update(entity);
        }


        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            
            _context
                .Set<T>()
                .Remove(entity);
        }


        public bool Exists(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            
            var result = _context
                .Set<T>()
                .Any(expression);

            return result;
        }
    }
}