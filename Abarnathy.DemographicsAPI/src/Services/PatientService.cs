using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Abarnathy.DemographicsAPI.Services.Interfaces;
using AutoMapper;
using System.Linq;

namespace Abarnathy.DemographicsAPI.Services
{
    /// <summary>
    /// Patient entity domain logic. 
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Asynchronously gets a Patient entity (including its relations) by ID
        /// and returns it as an InputModel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<PatientDTO> GetInputModelById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var entity = await _unitOfWork.PatientRepository.GetById(id);

            if (entity == null)
            {
                return null;
            }

            var result = _mapper.Map<PatientDTO>(entity);

            return result;
        }

        /// <summary>
        /// Asynchronously get all Patient entities (including their relations) by ID
        /// and returns it as an InputModel.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PatientDTO>> GetInputModelsAll()
        {
            var entities = await _unitOfWork.PatientRepository.GetAll();

            var result = _mapper.Map<IEnumerable<PatientDTO>>(entities);

            return result;
        }

        /// <summary>
        /// Asynchronously creates a new Patient entity and persists it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> Create(PatientDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new Patient();

            _mapper.Map(model, entity);

            if (model.Addresses.Any())
            {
                entity
            }

            try
            {
                _unitOfWork.PatientRepository.Create(entity);
                await _unitOfWork.CommitAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates a Patient entity and persists any changes made to the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Update(int id, PatientDTO model)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var entity = await _unitOfWork.PatientRepository.GetById(id);

            if (entity == null)
            {
                throw new Exception($"No Patient entity matching the ID <{id}> was found.");
            }

            try
            {
                _mapper.Map(model, entity);
                
                _unitOfWork
                    .PatientRepository
                    .Update(entity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}