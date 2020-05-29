using System;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Abarnathy.AssessmentService.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IConfiguration _configuration;
        private readonly IExternalHistoryAPIService _externalHistoryAPIService;

        public AssessmentService(IExternalHistoryAPIService externalHistoryAPIService, IConfiguration configuration)
        {
            _externalHistoryAPIService = externalHistoryAPIService;
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a <see cref="AssessmentResult"/> by analysing a patient's
        /// personal data and medical history.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public async Task<AssessmentResult> GenerateAssessment(PatientModel patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var triggerCount = await AssessNotes(patient.Id);

            var result = triggerCount == 0 ?
                new AssessmentResult(patient.Id,RiskLevel.None) :    
                AssesPersonalData(patient, triggerCount);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="triggerCount"></param>
        /// <returns></returns>
        private AssessmentResult AssesPersonalData(PatientModel patient,
            int triggerCount)
        {
            var patientAge = DateTime.Today.Year - patient.DateOfBirth.Year;

            var riskLevel = patientAge > 30
                ? AssessPatientUnder30(patient, triggerCount)
                : AssessPatientOver30(patient, triggerCount);

            var result = new AssessmentResult(patient.Id, riskLevel);
            
            return result;
        }

        private RiskLevel AssessPatientOver30(PatientModel patient, int triggerCount)
        {
            if (triggerCount < 2)
            {
                throw new ArgumentNullException(nameof(triggerCount));
            }
            
            RiskLevel result;
            
            switch (patient.SexId)
            {
                case 1 when triggerCount == 2:
                case 2 when triggerCount == 2:
                    result = RiskLevel.Borderline;
                    break;

                case 1 when triggerCount == 6:
                case 2 when triggerCount == 6:
                    result = RiskLevel.InDanger;
                    break;

                case 2 when triggerCount >= 8:
                    result = RiskLevel.EarlyOnset;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(patient.SexId));
            }

            return result;
        }
        
        private RiskLevel AssessPatientUnder30(PatientModel patient, int triggerCount)
        {
            if (triggerCount < 2)
            {
                throw new ArgumentNullException(nameof(triggerCount));
            }
            
            RiskLevel result;

            switch (patient.SexId)
            {
                case 1 when triggerCount == 3:
                case 2 when triggerCount == 4:
                    result = RiskLevel.InDanger;
                    break;

                case 1 when triggerCount == 5:
                case 2 when triggerCount == 7:
                    result = RiskLevel.EarlyOnset;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(patient.SexId));
            }

            return result;
        }

        /// <summary>
        /// Assesses a collection of <see cref="NoteModel"/> to find
        /// potential trigger terms. 
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private async Task<int> AssessNotes(int patientId)
        {
            var notes =
                await _externalHistoryAPIService.GetNotes(patientId);

            var triggers = 0;

            Log.Information(_configuration["TriggerTerms"]);
            
            foreach (var note in notes)
            {
                foreach (var term in _configuration["TriggerTerms"])
                {
                    if (note.Content.Normalize().Contains(
                        term.ToString().Normalize()))
                    {
                        triggers++;
                    }
                }
            }

            return triggers;
        }
    }
}