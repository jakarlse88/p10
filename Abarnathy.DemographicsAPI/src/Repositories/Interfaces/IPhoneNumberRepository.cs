using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public interface IPhoneNumberRepository : IRepositoryBase<PhoneNumber>
    {
        Task<PhoneNumber> GetByNumber(PhoneNumberInputModel model);
        new PhoneNumber Create(PhoneNumber entity);
    }
}
