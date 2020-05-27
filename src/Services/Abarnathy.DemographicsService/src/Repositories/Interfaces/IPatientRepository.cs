using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Models;

namespace Abarnathy.DemographicsService.Repositories
{
    public interface IPatientRepository : IRepositoryBase<Patient>
    {
        Task<Patient> GetById(int id);
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient> GetByFullPersonalia(PatientInputModel model);

        new Patient Create(Patient entity);
    }
}