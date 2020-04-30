using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Address> GetByCompleteAddressAsync(AddressDTO dto)
        {
            var result =
                await base.GetByCondition(a =>
                    a.StreetName.Contains(dto.StreetName) &&
                    a.HouseNumber.Contains(dto.HouseNumber) &&
                    a.State.Contains(dto.State) &&
                    a.Zipcode.Contains(dto.Zipcode))
                    .FirstOrDefaultAsync();

            return result;
        }
        
        /**
         * Helper methods
         * 
         **/
        private string NormaliseString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.Trim().ToLower();
        }
    }
}