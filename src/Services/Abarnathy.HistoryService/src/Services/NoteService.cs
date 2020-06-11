using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;
using Abarnathy.HistoryService.Repositories;
using AutoMapper;
using Serilog;

namespace Abarnathy.HistoryService.Services
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
        public async Task<IEnumerable<Note>> GetByPatientIdAsync(int patientId)
        {
            var result =
                await _noteRepository.GetByPatientIdAsync(patientId);

            return result;
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
                throw new ArgumentNullException(nameof(id));
            }
            
            var entity =
                await _noteRepository.GetByIdAsync(id);

            return entity;
        }

        /// <summary>
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Note> Create(NoteCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var entity = _mapper.Map<Note>(model);

            try
            {
                await _noteRepository.Insert(entity);
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
        public async Task<Note> Update(Note entity, NoteInputModel model)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                var logItem = new NoteLogItem
                {
                    TimeOriginallyCreated = entity.TimeLastUpdated,
                    TimeArchived = DateTime.Now,
                    Title = entity.Title,
                    Content = entity.Content
                };
                
                var newEntity = _mapper.Map<Note>(model);
                
                newEntity.TimeLastUpdated = DateTime.Now;
                newEntity.NoteLog.Add(logItem);
                
                return await _noteRepository.Update(entity.Id, newEntity);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred updating an entity: {0}", e.Message);
                throw;
            }
        }
    }
}