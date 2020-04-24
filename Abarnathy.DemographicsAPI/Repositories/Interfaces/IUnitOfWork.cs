using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes.
    /// </summary>
    public interface IUnitOfWork
    {
        Repository<Patient> PatientRepository { get; }
        Repository<Address> AddressRepository { get; }
        Repository<Sex> SexRepository { get; }
        Repository<PatientAddress> PatientAddressRepository { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}