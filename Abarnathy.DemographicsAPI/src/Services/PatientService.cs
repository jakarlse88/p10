using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Abarnathy.DemographicsAPI.Services.Interfaces;
using AutoMapper;

namespace Abarnathy.DemographicsAPI.Services
{
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
        public async Task<PatientInputModel> GetInputModelById(int id)
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

            var result = _mapper.Map<PatientInputModel>(entity);

            return result;
        }

        /// <summary>
        /// Asynchronously get all Patient entities (including their relations) by ID
        /// and returns it as an InputModel.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PatientInputModel>> GetInputModelsAll()
        {
            var entities = await _unitOfWork.PatientRepository.GetAll();

            var result = _mapper.Map<IEnumerable<PatientInputModel>>(entities);

            return result;
        }

        /// <summary>
        /// Asynchronously creates a new Patient entity and persists it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> Create(PatientInputModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var entity = _mapper.Map<Patient>(model);
            
            try
            {
                _unitOfWork.PatientRepository.Insert(entity);
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
        public async Task Update(int id, PatientInputModel model)
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