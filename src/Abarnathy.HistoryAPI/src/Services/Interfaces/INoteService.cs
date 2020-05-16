using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;

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
        public Task<IEnumerable<NoteInputModel>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Asynchronously get a single <see cref="Note"/> entity by its ID as a <seealso cref="NoteInputModel"/>
        /// DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<NoteInputModel?> GetByIdAsync(string id);

        /// <summary>
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Note Create(NoteInputModel model);
    }
}