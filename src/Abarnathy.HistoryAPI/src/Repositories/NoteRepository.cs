using System;
using System.Collections.Generic;
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
        /// Get a single <see cref="Note"/> entities by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Note> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = 
                await _context.Notes.FindAsync(n => n.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get any <see cref="Note"/> entity belonging to a
        /// specified Patient.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetByPatientIdAsync(int patientId)
        {
            var result = 
                await _context.Notes.FindAsync(n => n.PatientId == patientId);

            return result.ToList();
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
                throw new ArgumentNullException(nameof(entity));
            }
            
            await _context.Notes.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates a <see cref="Note"/> entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookIn"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Note> Update(string id, Note bookIn)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (bookIn == null)
            {
                throw new ArgumentNullException(nameof(bookIn));
            }

            return await _context.Notes.FindOneAndReplaceAsync(note => note.Id == id, bookIn);
        }
    }
}