using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Models;
using Abarnathy.DemographicsService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Abarnathy.DemographicsService.Controllers
{
    /// <summary>
    /// Patient route controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        /// <summary>
        /// <see cref="PatientController" /> class constructor.
        /// </summary>
        /// <param name="patientService"></param>
        public PatientController(IPatientService patientService)
        { 
            _patientService = patientService;
        }

        /// <summary>
        /// Gets all Patient entities as InputModels.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Request OK, return results.</response>
        /// <response code="404">No entities found.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PatientInputModel>>> Get()
        {
            var result = await _patientService.GetInputModelsAll();

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound("No entities found.");
        }

        /// <summary>
        /// Gets a single Patient entity as an InputModel by the entity's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Request OK, entity found.</response>
        /// <response code="400">Malformed request (bad ID).</response>
        /// <response code="404">Entity not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientInputModel>> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _patientService.GetInputModelById(id);

            if (result == null)
            {
                return NotFound();
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

            var createdEntity = await _patientService.Create(model);
            
            return CreatedAtAction("Get", new { createdEntity.Id }, createdEntity);
        }
        
        /// <summary>
        /// Updates a pre-existing <see cref="Patient"/> entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="204">The <see cref="Patient"/> entity was successfully updated.</response>
        /// <response code="400">Malformed request.</response>
        /// <response code="404">The specified entity could not be found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, PatientInputModel model)
        {
            if (id <= 0 || model == null)
            {
                return BadRequest();
            }

            var entity = await _patientService.GetEntityById(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _patientService.Update(entity, model);

            return NoContent();
        }

        /// <summary>
        /// Verifies whether a <see cref="Patient"/> entity identified by the specified
        /// ID exists in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Entity exists.</response>
        /// <response code="404">Entity not found.</response>
        [HttpGet("Exists/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PatientExists(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var exists = await _patientService.Exists(id);

            if (!exists)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
