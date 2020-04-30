using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Abarnathy.DemographicsAPI.Services.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        
        /**
         * Private helper methods
         * 
         */
        
        /// <summary>
        /// Links one or more <see cref="Address"/> entities to a <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task LinkAddresses(IEnumerable<AddressDTO> models, Patient entity)
        {
            if (models == null || entity == null)
            {
                throw new ArgumentNullException();
            }

            // Avoid multiple enumration
            var addressArray = models as AddressDTO[] ?? models.ToArray();
            
            if (addressArray.Any())
            {
                foreach (var addressDTO in addressArray)
                {
                    // Does a functionally identical entity already exist?
                    var result = await _unitOfWork.AddressRepository.GetByCompleteAddressAsync(addressDTO);

                    if (result == null) // No--create a new entity and link it up
                    {
                        var address = _mapper.Map<Address>(addressDTO);

                        _unitOfWork.AddressRepository.Create(address);

                        entity.PatientAddresses.Add(new PatientAddress
                        {
                            Patient = entity,
                            Address = address
                        });
                    }
                    else // Yes--link it up
                    {
                        entity.PatientAddresses.Add(new PatientAddress
                        {
                            Patient = entity,
                            Address = result
                        });
                    }
                    
                }
            }
        }

        /// <summary>
        /// Links one or more <see cref="PhoneNumber"/> entities to a <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task LinkPhoneNumbers(IEnumerable<PhoneNumberDTO> models, Patient entity)
        {
            if (models == null || entity == null)
            {
                throw new ArgumentNullException();
            }

            // Avoid multiple enumeration
            var phoneNumberDTOArray = models as PhoneNumberDTO[] ?? models.ToArray();

            if (phoneNumberDTOArray.Any())
            {
                foreach (var dto in phoneNumberDTOArray)
                {
                    // Does a functionally identical entity already exist?
                    var result = await _unitOfWork.PhoneNumberRepository.GetByNumber(dto);

                    if (result == null) // No--create a new entity and link it up
                    {
                        var phoneNumber = _mapper.Map<PhoneNumber>(dto);

                        //new Regex(@"^([- ().])+$").Replace(phoneNumber.Number, "");

                        phoneNumber.Number = Regex.Replace(phoneNumber.Number, @"[- ().]", "");

                        _unitOfWork.PhoneNumberRepository.Create(phoneNumber);

                        entity.PatientPhoneNumbers.Add(new PatientPhoneNumber
                        {
                            Patient = entity,
                            PhoneNumber = phoneNumber
                        });
                    }
                    else // Yes--link it up
                    {
                        entity.PatientPhoneNumbers.Add(new PatientPhoneNumber
                        {
                            Patient = entity,
                            PhoneNumber = result
                        });
                    }
                }
            }
        }
        
    }
}