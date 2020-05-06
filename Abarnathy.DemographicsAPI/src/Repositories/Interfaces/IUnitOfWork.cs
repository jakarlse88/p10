using System.Threading.Tasks;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes.
    /// </summary>
    public interface IUnitOfWork
    {
        IPatientRepository PatientRepository { get; }
        IAddressRepository AddressRepository { get; }
        IPhoneNumberRepository PhoneNumberRepository { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}