using System.Threading.Tasks;
using Abarnathy.DemographicsService.Models;

namespace Abarnathy.DemographicsService.Repositories
{
    public interface IPhoneNumberRepository : IRepositoryBase<PhoneNumber>
    {
        Task<PhoneNumber> GetByNumber(PhoneNumberInputModel model);
        new PhoneNumber Create(PhoneNumber entity);
    }
}
