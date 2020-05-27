using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;

namespace Abarnathy.HistoryService.Services
{
    /// <summary>
    /// <see cref="Note"/> domain logic.
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Asynchronously get all <see cref="Note"/> as <seealso cref="NoteInputModel"/> DTOs
        /// entities belonging to a certain Patient.
        /// </summary>
        /// <param name="patientId">ID of the Patient to search by.</param>
        /// <returns></returns>
        public Task<IEnumerable<Note>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Asynchronously get a single <see cref="Note"/> entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Note> GetByIdAsync(string id);

        /// <summary>
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<Note> Create(NoteCreateModel model);

        /// <summary>
        /// Updates a Note entity and persists any changes made to the DB.
        /// </summary>
        /// <param name="entity">The <see cref="Note"/> entity to update.</param>
        /// <param name="model">The <see cref="NoteInputModel"/> model containing the updated data.</param>
        public Task<Note> Update(Note entity, NoteInputModel model);
    }
}