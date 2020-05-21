using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using Abarnathy.HistoryAPI.Repositories;
using AutoMapper;
using Serilog;

namespace Abarnathy.HistoryAPI.Services
{
    /// <summary>
    /// <see cref="Note"/> domain logic. 
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly IMapper _mapper;
        private readonly INoteRepository _noteRepository;
        
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// /// <param name="noteRepository"></param>
        public NoteService(IMapper mapper, INoteRepository noteRepository)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
        }

        /// <summary>
        /// Asynchronously get all <see cref="Note"/> as <seealso cref="NoteInputModel"/> DTOs
        /// entities belonging to a certain Patient.
        /// </summary>
        /// <param name="patientId">ID of the Patient to search by.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<NoteInputModel>> GetByPatientIdAsInputModelAsync(int patientId)
        {
            var tempResult = 
                await _noteRepository.GetByConditionAsync(n => n.PatientId == patientId);

            if (!tempResult.Any())
            {
                return new List<NoteInputModel>();
            }
            
            var result = _mapper.Map<IEnumerable<NoteInputModel>>(tempResult);

            return result;
        }

        /// <summary>
        /// Asynchronously get a single <see cref="Note"/> entity by its ID as a <seealso cref="NoteInputModel"/>
        /// DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<NoteInputModel> GetByIdAsInputModelAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            
            var entity =
                await _noteRepository.GetSingleByIdAsync(id);

            if (entity != null)
            {
                var model = _mapper.Map<NoteInputModel>(entity);

                return model;             
            }

            return null;
        }

        /// <summary>
        /// Asynchronously get a single <see cref="Note"/> entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Note> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            
            var entity =
                await _noteRepository.GetSingleByIdAsync(id);

            return entity;
        }

        /// <summary>
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Note Create(NoteCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            
            var entity = _mapper.Map<Note>(model);

            try
            {
                _noteRepository.Insert(entity);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred inserting an entity: {0}", e.Message);
                throw;
            }
            
            return entity;
        }

        /// <summary>
        /// Updates a Note entity and persists any changes made to the DB.
        /// </summary>
        /// <param name="entity">The <see cref="Note"/> entity to update.</param>
        /// <param name="model">The <see cref="NoteInputModel"/> model containing the updated data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Update(Note entity, NoteInputModel model)
        {
            if (entity == null || model == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var newEntity = _mapper.Map<Note>(model);
                await _noteRepository.Update(entity.Id, newEntity);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred updating an entity: {0}", e.Message);
                throw;
            }
        }
    }
}