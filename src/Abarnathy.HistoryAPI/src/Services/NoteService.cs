using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Data;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using Abarnathy.HistoryAPI.Repositories;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
        public async Task<IEnumerable<NoteInputModel>> GetByPatientIdAsync(int patientId)
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
        public async Task<NoteInputModel?> GetByIdAsync(string id)
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
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Note Create(NoteInputModel model)
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

        // public void Update(string id, Note bookIn) =>
        //     _books.ReplaceOne(note => note.Id == id, bookIn);

        // public void Remove(Note bookIn) =>
        //     _books.DeleteOne(note => note.Id == bookIn.Id);

        // public void Remove(string id) =>
        //     _books.DeleteOne(note => note.Id == id);
    }
}