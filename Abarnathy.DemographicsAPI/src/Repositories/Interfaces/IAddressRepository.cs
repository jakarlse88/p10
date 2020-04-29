using Abarnathy.DemographicsAPI.Models;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        Task<Address> GetByCompleteAddressAsync(AddressDTO dto);
    }
}
