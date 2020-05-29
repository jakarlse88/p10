using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;

namespace Abarnathy.AssessmentService.Services
{
    public interface IExternalHistoryAPIService
    {
        Task<IEnumerable<NoteModel>> GetNotes(int patientId);
    }
}