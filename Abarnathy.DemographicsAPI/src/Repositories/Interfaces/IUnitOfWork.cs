using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes.
    /// </summary>
    public interface IUnitOfWork
    {
        IPatientRepository PatientRepository { get; }
        Task CommitAsync();
        Task RollbackAsync();
    }
}