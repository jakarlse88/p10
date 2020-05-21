using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using Abarnathy.HistoryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Abarnathy.HistoryAPI.Controllers
{
    /// <summary>
    /// api/note endpoint.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IExternalService _externalService;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="noteService"></param>
        /// <param name="externalService"></param>
        public HistoryController(INoteService noteService, IExternalService externalService)
        {
            _noteService = noteService;
            _externalService = externalService;
        }

        /// <summary>
        /// Gets a single <see cref="Note"/> entity by its ID.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <response code="200">Request OK, return results.</response>
        /// <response code="204">Request OK, no results.</response>
        /// <response code="400">Malformed request (ID null).</response>
        [HttpGet("note/{noteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NoteInputModel>> GetByNoteId(string noteId)
        {
            if (string.IsNullOrWhiteSpace(noteId))
            {
                return BadRequest();
            }

            var result = await _noteService.GetByIdAsInputModelAsync(noteId);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all <see cref="Note"/> entities related to
        /// a given Patient entity.
        /// </summary>
        /// <param name="patientId">ID of the Patient entity.</param>
        /// <returns></returns>
        /// <response code="200">Request OK, return results.</response>
        /// <response code="204">Request OK, no results.</response>
        [HttpGet("patient/{patientId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<NoteInputModel>>> GetByPatientId(int patientId)
        {
            if (!await _externalService.PatientExists(patientId))
            {
                return BadRequest();
            }

            var result = await _noteService.GetByPatientIdAsInputModelAsync(patientId);

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Create a new <see cref="Note"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="201">Request OK, Note created.</response>
        /// <response code="400">Malformed request.</response>
        [HttpPost("note/")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> Post(NoteCreateModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            await _externalService.PatientExists(model.PatientId);

            var result = _noteService.Create(model);

            return CreatedAtAction(nameof(GetByNoteId), new { noteId = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing <see cref="Note"/> entity. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("note/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(string id, NoteInputModel model)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                model == null ||
                !await _externalService.PatientExists(model.PatientId))
            {
                return BadRequest();
            }

            var entity = await _noteService.GetByIdAsync(id);

            if (entity == null)
            {
                return BadRequest($"No Note with the ID [{id}] was found. Update aborted.");
            }

            await _noteService.Update(entity, model);

            return NoContent();
        }
    }
}