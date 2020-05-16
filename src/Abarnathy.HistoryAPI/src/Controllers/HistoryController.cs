using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using Abarnathy.HistoryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

            /// <summary>
            /// Class constructor.
            /// </summary>
            /// <param name="noteService"></param>
            public HistoryController(INoteService noteService)
            {
                _noteService = noteService;
            }

            /// <summary>
            /// Gets a single <see cref="Note"/> entity by its ID.
            /// </summary>
            /// <param name="noteId"></param>
            /// <returns></returns>
            /// <response code="200">Request OK, return results.</response>
            /// <response code="204">Request OK, no results.</response>
            /// <response code="400">Malformed request (ID null).</response>
            [HttpGet("note/{noteId}"), ActionName("GetById")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<NoteInputModel>> GetById(string noteId)
            {
                if (string.IsNullOrWhiteSpace(noteId))
                {
                    return BadRequest();
                }
                
                var result = await _noteService.GetByIdAsync(noteId);
            
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
            public async Task<ActionResult<IEnumerable<NoteInputModel>>> Get(int patientId)
            {
                var result = await _noteService.GetByPatientIdAsync(patientId);
            
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
            [HttpPost("note/")]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public ActionResult<Note> Post([FromBody] NoteInputModel model)
            {
                if (model == null)
                {
                    return BadRequest();
                }
                
                var result = _noteService.Create(model);
            
                return CreatedAtAction(nameof(GetById), new { noteId = model.Id }, result);
            }
    }
}