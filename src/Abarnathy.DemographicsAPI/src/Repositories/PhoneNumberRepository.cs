using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// <see cref="PhoneNumber"/> data access implementation.
    /// </summary>
    public class PhoneNumberRepository : RepositoryBase<PhoneNumber>, IPhoneNumberRepository
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        public PhoneNumberRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a <see cref="PhoneNumber"/> entity by its <seealso cref="PhoneNumber.Number"/> property.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<PhoneNumber> GetByNumber(PhoneNumberInputModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Number))
            {
                throw new ArgumentNullException();
            }

            var numberToCompare = Regex.Replace(model.Number, @"[- ().]", "");
            
            var result =
                await base.GetByCondition(pn =>
                    pn.Number.Contains(numberToCompare))
                .FirstOrDefaultAsync();

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
        public new PhoneNumber Create(PhoneNumber entity)
        {
            if (entity == null ||
                string.IsNullOrWhiteSpace(entity.Number))
            {
                throw new ArgumentNullException();
            }

            base.Create(entity);

            return entity;
        }
    }
}
