using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientInputModel> GetById(int id);
        Task<IEnumerable<PatientInputModel>> GetAll();
        Task<int> Create(PatientInputModel model);
        Task Update(int id, PatientInputModel model);
    }
}