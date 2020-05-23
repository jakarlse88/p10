using System;
using System.Threading.Tasks;

namespace Abarnathy.HistoryAPI.Services
{
    /// <summary>
    /// External API interaction.
    /// </summary>
    public interface IExternalAPIService
    {
        /// <summary>
        /// Call the DemographicsAPI to ensure that the Patient entity exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<bool> PatientExists(int id);
    }
}