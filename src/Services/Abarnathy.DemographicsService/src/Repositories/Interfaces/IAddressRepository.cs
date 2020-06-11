using System.Threading.Tasks;
using Abarnathy.DemographicsService.Models;

namespace Abarnathy.DemographicsService.Repositories
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        Task<Address> GetByCompleteAddressAsync(AddressInputModel model);
        new Address Create(Address entity);
    }
}
