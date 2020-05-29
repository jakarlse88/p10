using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Abarnathy.AssessmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly IExternalDemographicsAPIService _externalDemographicsAPIService;
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IExternalDemographicsAPIService externalDemographicsAPIService, IAssessmentService assessmentService)
        {
            _externalDemographicsAPIService = externalDemographicsAPIService;
            _assessmentService = assessmentService;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("Patient/{patientId}")]
        public async Task<IActionResult> Get(int patientId)
        {
            var patient = 
                await _externalDemographicsAPIService.GetPatient(patientId);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var assessmentResult = 
                await _assessmentService.GenerateAssessment(patient);

            return Ok(assessmentResult);
        }
    }
}