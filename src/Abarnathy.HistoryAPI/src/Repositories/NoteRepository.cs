using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Data;
using Abarnathy.HistoryAPI.Models;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Repositories
{
    /// <summary>
    /// <see cref="Note"/> DAL.
    /// </summary>
    public class NoteRepository : INoteRepository
    {
        private readonly PatientHistoryDbContext _context;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="context"></param>
        public NoteRepository(PatientHistoryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all <see cref="Note"/> entities matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<Note>> GetByConditionAsync(Expression<Func<Note, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException();
            }

            var tempResult = await _context.Notes.FindAsync(n => true);

            IEnumerable<Note> result = tempResult.ToList();

            return result;
        }

        /// <summary>
        /// Get a single <see cref="Note"/> entities by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Note> GetSingleByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException();
            }

            var result = await _context.Notes.FindAsync(n => n.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a new <see cref="Note"/> entity into the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Insert(Note entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            
            await _context.Notes.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates a <see cref="Note"/> entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookIn"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Update(string id, Note bookIn)
        {
            if (string.IsNullOrWhiteSpace(id) || bookIn == null)
            {
                throw new ArgumentNullException();
            }

            await _context.Notes.FindOneAndReplaceAsync(note => note.Id == id, bookIn);
        }
    }
}