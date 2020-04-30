using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public interface IPatientRepository : IRepositoryBase<Patient>
    {
        Task<Patient> GetById(int id);
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient> GetByFullPersonalia(PatientDTO model);

    }
}