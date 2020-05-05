using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class PatientAddressRepository : RepositoryBase<PatientAddress>, IPatientAddressRepository
    {
        public PatientAddressRepository(DemographicsDbContext context) : base(context)
        {
        }
    }
}