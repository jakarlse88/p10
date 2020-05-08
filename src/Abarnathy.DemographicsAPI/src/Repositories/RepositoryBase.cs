using System;
using System.Linq;
using System.Linq.Expressions;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DemographicsDbContext _context;

        public RepositoryBase(DemographicsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a subset of entity T by a given condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException();
            }

            var result = _context
                .Set<TEntity>()
                .Where(predicate);

            return result;
        }

        /// <summary>
        /// Begin tracking the entity in the Added state.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            _context
                .Set<TEntity>()
                .Add(entity);
        }

        /// <summary>
        /// Begin tracking the entity in the Modified state (generally).
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            _context
                .Set<TEntity>()
                .Update(entity);
        }

        /// <summary>
        /// Begins tracking the entity in the Deleted state.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            _context
                .Set<TEntity>()
                .Remove(entity);
        }
    }
}