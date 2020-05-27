using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Models;

namespace Abarnathy.HistoryService.Repositories
{
    /// <summary>
    /// <see cref="Note"/> DAL.
    /// </summary>
    public interface INoteRepository
    {
        /// <summary>
        /// Get a single <see cref="Note"/> entities by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Note> GetByIdAsync(string id);

        /// <summary>
        /// Get any <see cref="Note"/> entity belonging to a
        /// specified Patient.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public Task<IEnumerable<Note>> GetByPatientIdAsync(int patientId);

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
        public Task<Note> Update(string id, Note bookIn);
    }
}