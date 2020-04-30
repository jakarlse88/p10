using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class PhoneNumberRepository : RepositoryBase<PhoneNumber>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a <see cref="PhoneNumber"/> entity by its <seealso cref="PhoneNumber.Number"/> property.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PhoneNumber> GetByNumber(PhoneNumberDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Number))
            {
                throw new ArgumentNullException();
            }

            var result =
                await base.GetByCondition(pn =>
                    pn.Number.Contains(dto.Number))
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
