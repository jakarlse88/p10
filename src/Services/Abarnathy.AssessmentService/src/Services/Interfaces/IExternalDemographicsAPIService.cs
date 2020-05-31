using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;

namespace Abarnathy.AssessmentService.Services
{
    public interface IExternalDemographicsAPIService
    {
        Task<PatientModel> GetPatientAsync(int patientId);
        
    }
}