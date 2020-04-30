using Abarnathy.DemographicsAPI.Models;
using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories.Interfaces
{
    public interface IPhoneNumberRepository : IRepositoryBase<PhoneNumber>
    {
        Task<PhoneNumber> GetByNumber(PhoneNumberDTO dto);
    }
}
