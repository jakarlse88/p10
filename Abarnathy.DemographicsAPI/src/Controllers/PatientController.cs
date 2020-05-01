using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Abarnathy.DemographicsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Gets all Patient entities as InputModels.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Request OK, return results.</response>
        /// <response code="204">Request OK, no results to return.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<PatientInputModel>>> Get()
        {
            var result = await _patientService.GetInputModelsAll();

            if (result.Any())
            {
                return Ok(result);
            }

            return NoContent();
        }

        /// <summary>
        /// Gets a single Patient entity as an InputModel by the entity's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Request OK, entity found.</response>
        /// <response code="204">Request OK, no entity found.</response>
        /// <response code="400">Malformed request (bad ID).</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientInputModel>> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _patientService.GetInputModelById(id);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new Patient entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="201">The entity was successfully created.</response>
        /// <response code="400">Malformed request (arg null).</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(PatientInputModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var resultId = await _patientService.Create(model);
            
            return CreatedAtAction("Get", new { Id = resultId });
        }
    }
}
