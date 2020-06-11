using System;

namespace Abarnathy.AssessmentService.Models
{
    public class AssessmentResult
    {
        public AssessmentResult(int patientId, RiskLevel riskLevel)
        {
            PatientId = patientId;
            RiskLevel = riskLevel;
            TimeCreated = DateTime.Now;
        }
        
        public int PatientId { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}