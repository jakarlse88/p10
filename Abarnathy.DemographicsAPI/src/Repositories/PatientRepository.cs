using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public PatientRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Patient" /> entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<Patient> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var result =
                await
                    base.GetByCondition(p => p.Id == id)
                        .Include(p => p.PatientAddresses)
                        .ThenInclude(pa => pa.Address)
                        .Include(p => p.Sex)
                        .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Asynchronously gets all <see cref="Patient" /> entities.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Patient>> GetAll()
        {
            var result =
                await base.GetByCondition(p => true)
                    .Include(p => p.PatientAddresses)
                    .ThenInclude(pa => pa.Address)
                    .Include(p => p.Sex)
                    .ToListAsync();

            return result;
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Patient" /> entity by the minimal set
        /// of properties that uniquely identify it: FamilyName, GivenName, DateOfBirth, and SexId. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Patient> GetByFullPersonalia(PatientInputModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var result =
                await base.GetByCondition(p =>
                        p.FamilyName.Contains(model.FamilyName) &&
                        p.GivenName.Contains(model.GivenName))
                    .FirstOrDefaultAsync(p => p.SexId == model.SexId && p.DateOfBirth.Date == model.DateOfBirth.Date);

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Patient" /> entity with its minimally complete set of properties
        /// (ie. validates that the properties are not null or whitespace and begins tracking the entity
        /// in the 'Added' state).
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public new Patient Create(Patient entity)
        {
            if (entity == null ||
                string.IsNullOrEmpty(entity.FamilyName) ||
                string.IsNullOrEmpty(entity.GivenName))
            {
                throw new ArgumentNullException();
            }

            if (entity.SexId < 1 || entity.SexId > 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            base.Create(entity);

            return entity;
        }
    }
}