using System;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// <see cref="Address"/> data access layer implementation.
    /// </summary>
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        public AddressRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Finds a <see cref="Address"/> entity by its minimally complete set of properties.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Address> GetByCompleteAddressAsync(AddressInputModel model)
        {
            var result =
                await base.GetByCondition(a =>
                        a.StreetName.Contains(model.StreetName) &&
                        a.HouseNumber.Contains(model.HouseNumber) &&
                        a.Town.Contains(model.Town) &&
                        a.State.Contains(model.State) &&
                        a.ZipCode.Contains(model.ZipCode))
                    .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Address" /> entity with its minimally complete set of properties
        /// (ie. validates that the properties are not null or whitespace and begins tracking the entity
        /// in the 'Added' state).
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public new Address Create(Address entity)
        {
            if (entity == null ||
                string.IsNullOrEmpty(entity.StreetName) ||
                string.IsNullOrEmpty(entity.HouseNumber) ||
                string.IsNullOrEmpty(entity.Town) ||
                string.IsNullOrEmpty(entity.State) ||
                string.IsNullOrEmpty(entity.ZipCode))
            {
                throw new ArgumentNullException();
            }

            base.Create(entity);

            return entity;
        }
    }
}