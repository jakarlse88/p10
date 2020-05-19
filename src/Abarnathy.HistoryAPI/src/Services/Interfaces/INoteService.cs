using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Services
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
        public Task<IEnumerable<NoteInputModel>> GetByPatientIdAsInputModelAsync(int patientId);

        /// <summary>
        /// Asynchronously get a single <see cref="Note"/> entity by its ID as a <seealso cref="NoteInputModel"/>
        /// DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<NoteInputModel> GetByIdAsInputModelAsync(string id);

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
        public Note Create(NoteCreateModel model);

        /// <summary>
        /// Updates a Note entity and persists any changes made to the DB.
        /// </summary>
        /// <param name="entity">The <see cref="Note"/> entity to update.</param>
        /// <param name="model">The <see cref="NoteInputModel"/> model containing the updated data.</param>
        public Task Update(Note entity, NoteInputModel model);
    }
}