using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Repositories
{
    /// <summary>
    /// <see cref="Note"/> DAL.
    /// </summary>
    public interface INoteRepository
    {
        /// <summary>
        /// Get all <see cref="Note"/> entities matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<IEnumerable<Note>> GetByConditionAsync(Expression<Func<Note, bool>> predicate);

        /// <summary>
        /// Get a single <see cref="Note"/> entities by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Note> GetSingleByIdAsync(string id);

        /// <summary>
        /// Inserts a new <see cref="Note"/> entity into the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task Insert(Note entity);

        /// <summary>
        /// Updates a <see cref="Note"/> entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookIn"></param>
        public Task Update(string id, Note bookIn);
    }
}