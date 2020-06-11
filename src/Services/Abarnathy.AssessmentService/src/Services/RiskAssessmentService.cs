using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Microsoft.Extensions.Configuration;

namespace Abarnathy.AssessmentService.Services
{
    public class RiskAssessmentService : IAssessmentService
    {
        private readonly IExternalHistoryAPIService _externalHistoryAPIService;
        private readonly IConfiguration _configuration;

        public RiskAssessmentService(IExternalHistoryAPIService externalHistoryAPIService, IConfiguration configuration)
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
                new AssessmentResult(patient.Id, RiskLevel.None) :
                AssesPersonalData(patient, triggerCount);

            return result;
        }

        /// <summary>
        /// Assesses a Patient's age and no. of trigger terms to determine
        /// the assessment result. 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="triggerCount"></param>
        /// <returns></returns>
        private AssessmentResult AssesPersonalData(PatientModel patient,
            int triggerCount)
        {
            var patientAge = GetAge(patient);

            var riskLevel = patientAge > 30
                ? AssessPatientOver30(patient, triggerCount)
                : AssessPatientUnder30(patient, triggerCount);

            var result = new AssessmentResult(patient.Id, riskLevel);

            return result;
        }

        /// <summary>
        /// Computes the <see cref="RiskLevel"/> of a given patient under 30.
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="triggerCount"></param>
        /// <returns></returns>
        private RiskLevel AssessPatientOver30(PatientModel patient, int triggerCount)
        {
            if (triggerCount <= 0)
            {
                throw new ArgumentNullException(nameof(triggerCount));
            }

            RiskLevel result;

            switch (patient.SexId)
            {
                case 1 when triggerCount >= 8:
                case 2 when triggerCount >= 8:
                    result = RiskLevel.EarlyOnset;
                    break;

                case 1 when triggerCount >= 6:
                case 2 when triggerCount >= 6:
                    result = RiskLevel.InDanger;
                    break;

                case 1 when triggerCount >= 2:
                case 2 when triggerCount >= 2:
                    result = RiskLevel.Borderline;
                    break;

                default:
                    result = RiskLevel.None;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Computes the <see cref="RiskLevel"/> of a patient over 30. 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="triggerCount"></param>
        /// <returns></returns>
        private RiskLevel AssessPatientUnder30(PatientModel patient, int triggerCount)
        {
            if (triggerCount <= 0)
            {
                throw new ArgumentNullException(nameof(triggerCount));
            }

            RiskLevel result;

            switch (patient.SexId)
            {
                case 1 when triggerCount >= 5:
                case 2 when triggerCount >= 7:
                    result = RiskLevel.EarlyOnset;
                    break;

                case 1 when triggerCount >= 3:
                case 2 when triggerCount >= 4:
                    result = RiskLevel.InDanger;
                    break;

                default:
                    result = RiskLevel.None;
                    break;
            }

            return result;
        }

        // https://stackoverflow.com/a/1404
        /// <summary>
        /// Gets a patients age (accounting for leap years).
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private int GetAge(PatientModel patient)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - patient.DateOfBirth.Year;
            // Go back to the year the person was born in case of a leap year
            if (patient.DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
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
                await _externalHistoryAPIService.GetPatientHistoryAsync(patientId);

            var terms = _configuration.GetSection("TriggerTerms").GetChildren().ToList();

            var confirmedTerms = new List<string>();

            foreach (var note in notes)
            {
                foreach (var term in terms)
                {
                    if (!confirmedTerms.Any(t => t.Contains(term.Value, StringComparison.OrdinalIgnoreCase)) &&
                        (note.Content.Contains(term.Value, StringComparison.OrdinalIgnoreCase)))
                    {
                        confirmedTerms.Add(term.Value);
                    }
                }
            }

            return confirmedTerms.Count;
        }
    }
}