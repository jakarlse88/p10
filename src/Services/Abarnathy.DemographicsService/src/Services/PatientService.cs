using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Infrastructure;
using Abarnathy.DemographicsService.Models;
using Abarnathy.DemographicsService.Repositories;
using Abarnathy.DemographicsService.Services.Interfaces;

namespace Abarnathy.DemographicsService.Services
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
        /// Asynchronously gets a <see cref="Patient"/> entity (including its relation) by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<Patient> GetEntityById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var entity = await _unitOfWork.PatientRepository.GetById(id);

            return entity;
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Patient"/> entity (including its relations) by ID
        /// and returns it as an <see cref="PatientInputModel"/>.
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
        public async Task<Patient> Create(PatientInputModel model)
        {
            if (model?.Addresses == null || model.PhoneNumbers == null)
            {
                throw new ArgumentNullException();
            }

            var patient = await _unitOfWork.PatientRepository.GetByFullPersonalia(model);

            if (patient != null)
            {
                throw new Exception("Error: a Patient entity already exists that matches the supplied personalia.");
            }

            var entity = _mapper.Map<Patient>(model);

            await LinkAddresses(model.Addresses, entity);
            await LinkPhoneNumbers(model.PhoneNumbers, entity);

            try
            {
                _unitOfWork.PatientRepository.Create(entity);
                await _unitOfWork.CommitAsync();
                return entity;
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
        /// <param name="entity">The <see cref="Patient"/> entity to update.</param>
        /// <param name="model">The <see cref="PatientInputModel"/> model containing the updated data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Update(Patient entity, PatientInputModel model)
        {
            if (entity == null || model == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                _mapper.Map(model, entity);

                await UpdateAddresses(model.Addresses, entity);
                await UpdatePhoneNumbers(model.PhoneNumbers, entity);

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
        
        /// <summary>
        /// Verifies whether a <see cref="Patient"/> entity identified by the specified
        /// ID exists in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<bool> Exists(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var result = await _unitOfWork.PatientRepository.GetById(id);

            if (result == null)
            {
                return false;
            }

            return true;
        }

        /**
         * ====================================================
         * Private helper methods
         * ==================================================== 
         */
        
        /// <summary>
        /// Links one or more <see cref="Address"/> entities to a <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="modelEnumerable"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task LinkAddresses(IEnumerable<AddressInputModel> modelEnumerable, Patient entity)
        {
            if (modelEnumerable == null || entity == null)
            {
                throw new ArgumentNullException();
            }

            // Avoid multiple enumeration
            var modelArray = modelEnumerable as AddressInputModel[] ?? modelEnumerable.ToArray();

            if (modelArray.Any())
            {
                foreach (var model in modelArray)
                {
                    await HandleIncomingAddress(entity, model);
                }
            }
        }

        /// <summary>
        /// Handle an incoming <see cref="AddressInputModel"/> DTO.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task HandleIncomingAddress(Patient entity, AddressInputModel model)
        {
            if (entity == null || model == null || AddressInputModelPropertiesNullOrWhitespace(model))
            {
                throw new ArgumentNullException();
            }
            
            // Does a functionally identical entity already exist?
            var result = await _unitOfWork.AddressRepository.GetByCompleteAddressAsync(model);

            // No--create a new entity and link it up
            if (result == null) 
            {
                var address = _mapper.Map<Address>(model);

                _unitOfWork.AddressRepository.Create(address);

                entity.PatientAddresses.Add(new PatientAddress
                {
                    Patient = entity,
                    Address = address
                });
            }
            // Yes--link it up if it isn't already linked (if it is already linked, no need to do anything)
            else if (!entity.PatientAddresses.Any(pa => pa.PatientId == entity.Id && pa.AddressId == result.Id)) 
            {
                entity.PatientAddresses.Add(new PatientAddress
                {
                    Patient = entity,
                    Address = result
                });
            }
        }

        /// <summary>
        /// Indicates whether the properties of an <see cref="PatientInputModel"/> DTO are null or whitespace.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static bool AddressInputModelPropertiesNullOrWhitespace(AddressInputModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            
            return
                string.IsNullOrWhiteSpace(model.StreetName) ||
                string.IsNullOrWhiteSpace(model.HouseNumber) ||
                string.IsNullOrWhiteSpace(model.Town) ||
                string.IsNullOrWhiteSpace(model.State) ||
                string.IsNullOrWhiteSpace(model.ZipCode);
        }

        /// <summary>
        /// Links one or more <see cref="PhoneNumber"/> entities to a <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="modelEnumerable"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task LinkPhoneNumbers(IEnumerable<PhoneNumberInputModel> modelEnumerable, Patient entity)
        {
            if (modelEnumerable == null || entity == null)
            {
                throw new ArgumentNullException();
            }

            // Avoid multiple enumeration
            var modelArray = modelEnumerable as PhoneNumberInputModel[] ?? modelEnumerable.ToArray();

            if (modelArray.Any())
            {
                foreach (var model in modelArray)
                {
                    await HandleIncomingPhoneNumber(entity, model);
                }
            }
        }

        /// <summary>
        /// Handle an incoming <see cref="PhoneNumberInputModel"/> DTO.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task HandleIncomingPhoneNumber(Patient entity, PhoneNumberInputModel model)
        {
            if (entity == null || model == null || string.IsNullOrEmpty(model.Number))
            {
                throw new ArgumentNullException();
            }

            // Does a functionally identical entity already exist?
            var result = await _unitOfWork.PhoneNumberRepository.GetByNumber(model);

            if (result == null) // No--create a new entity and link it up
            {
                var phoneNumber = _mapper.Map<PhoneNumber>(model);

                // Get rid of symbols
                // phoneNumber.Number = Regex.Replace(phoneNumber.Number, @"[- ().]", "");

                _unitOfWork.PhoneNumberRepository.Create(phoneNumber);

                entity.PatientPhoneNumbers.Add(new PatientPhoneNumber
                {
                    Patient = entity,
                    PhoneNumber = phoneNumber
                });
            }
            // Yes--link it up if it isn't already linked (if it is already linked, no need to do anything)
            else if (!entity.PatientPhoneNumbers.Any(pa => pa.PatientId == entity.Id && pa.PhoneNumberId == result.Id))
            {
                entity.PatientPhoneNumbers.Add(new PatientPhoneNumber
                {
                    Patient = entity,
                    PhoneNumber = result
                });
            }
        }

        /// <summary>
        /// Updates the <see cref="Address"/> entities associated with
        /// a given <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="modelEnumerable"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task UpdateAddresses(IEnumerable<AddressInputModel> modelEnumerable, Patient entity)
        {
            if (modelEnumerable == null || entity == null)
            {
                throw new ArgumentNullException();
            }

            // Avoid multiple enumeration
            var modelArray = modelEnumerable as AddressInputModel[] ?? modelEnumerable.ToArray();

            // Keep a reference to the old collection and instantiate a new one
            var deprecatedPatientAddressCollection = entity.PatientAddresses;
            entity.PatientAddresses = new HashSet<PatientAddress>();
            
            // Handle any incoming addresses
            if (modelArray.Any())
            {
                foreach (var model in modelArray)
                {
                    await HandleIncomingAddress(entity, model);
                }
            }
            // There are no incoming addresses but there are existing relations,
            // ie. these need to be removed
            else if (deprecatedPatientAddressCollection.Any()) 
            {
                foreach (var item in entity.PatientAddresses)
                {
                    // If there is only a single relation (ie. the current Patient), the Address entity should
                    // be deleted. Deleting either a Patient or Address entity cascades to the junction table,
                    // so this deletion should also serve to unlink the Address from the Patient.
                    if (item.Address.PatientAddresses.Count == 1)
                    {
                        _unitOfWork.AddressRepository.Delete(item.Address);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the <see cref="PhoneNumber"/> entities associated with a given
        /// <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="modelEnumerable"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task UpdatePhoneNumbers(IEnumerable<PhoneNumberInputModel> modelEnumerable, Patient entity)
        {
            if (modelEnumerable == null || entity == null)
            {
                throw new ArgumentNullException();
            }
            
            // Avoid multiple enumeration
            var modelSet = modelEnumerable.DistinctBy(x => x.Number);
            var phoneNumberInputModels = modelSet as PhoneNumberInputModel[] ?? modelSet.ToArray();
            
            // Keep a reference to the old collection and instantiate a new one
            var deprecatedPatientPhoneNumberCollection = entity.PatientPhoneNumbers;
            entity.PatientPhoneNumbers = new HashSet<PatientPhoneNumber>();
            
            // Handle any incoming phone numbers
            if (phoneNumberInputModels.Any())
            {
                foreach (var model in phoneNumberInputModels)
                {
                    await HandleIncomingPhoneNumber(entity, model);
                }
            }
            // There are no incoming phone numbers but there are existing relations,
            // ie. these need to be removed
            else if (deprecatedPatientPhoneNumberCollection.Any())
            {
                foreach (var item in entity.PatientPhoneNumbers)
                {
                    // If there is only a single relation (ie. the current Patient), the PhoneNumber entity should
                    // be deleted. Deleting either a Patient or Address entity cascades to the junction table,
                    // so this deletion should also serve to unlink the Address from the Patient.
                    if (item.PhoneNumber.PatientPhoneNumbers.Count == 1)
                    {
                        _unitOfWork.PhoneNumberRepository.Delete(item.PhoneNumber);
                    }
                }
            }
        }
    }
}