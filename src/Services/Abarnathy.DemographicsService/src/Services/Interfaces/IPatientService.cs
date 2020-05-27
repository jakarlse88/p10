using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Models;

namespace Abarnathy.DemographicsService.Services.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> GetEntityById(int id);
        Task<PatientInputModel> GetInputModelById(int id);
        Task<IEnumerable<PatientInputModel>> GetInputModelsAll();
        Task<Patient> Create(PatientInputModel model);
        Task Update(Patient entity, PatientInputModel model);
        Task<bool> Exists(int id);
    }
}