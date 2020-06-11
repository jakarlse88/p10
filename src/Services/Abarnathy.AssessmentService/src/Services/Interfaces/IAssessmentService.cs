using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;

namespace Abarnathy.AssessmentService.Services
{
    public interface IAssessmentService
    {
        Task<AssessmentResult> GenerateAssessment(PatientModel patient);
    }
}