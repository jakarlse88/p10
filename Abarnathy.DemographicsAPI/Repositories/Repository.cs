using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abarnathy.DemographicsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DemographicsDbContext _context;

        public Repository(DemographicsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll() =>
            _context
                .Set<T>()
                .AsNoTracking()
                .AsEnumerable();

        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
            _context
                .Set<T>()
                .Where(expression)
                .AsNoTracking()
                .AsEnumerable();

        public void Insert(T entity) =>
            _context
                .Set<T>()
                .Add(entity);

        public void Update(T entity) =>
            _context
                .Set<T>()
                .Update(entity);

        public void Delete(T entity) =>
            _context
                .Set<T>()
                .Remove(entity);

        public bool Exists(Expression<Func<T, bool>> expression) =>
            _context
                .Set<T>()
                .Any(expression);
    }
}