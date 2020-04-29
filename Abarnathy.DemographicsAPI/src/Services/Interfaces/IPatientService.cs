using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDTO> GetInputModelById(int id);
        Task<IEnumerable<PatientDTO>> GetInputModelsAll();
        Task<int> Create(PatientDTO model);
        Task Update(int id, PatientDTO model);
    }
}