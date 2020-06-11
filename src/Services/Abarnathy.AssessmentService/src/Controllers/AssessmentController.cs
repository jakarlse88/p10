using System.Threading.Tasks;
using Abarnathy.AssessmentService.Services;
using Microsoft.AspNetCore.Http;
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
        /// Gets the diabetes risk assessment for a given patient.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        /// <response code="200">Request OK, returns assessment.</response>
        /// <response code="404">Patient not found..</response>
        [HttpGet("Patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int patientId)
        {
            var patient = 
                await _externalDemographicsAPIService.GetPatientAsync(patientId);

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