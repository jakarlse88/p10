using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using Abarnathy.HistoryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Newtonsoft.Json;

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
            public async Task<ActionResult<IEnumerable<NoteInputModel>>> Get(int patientId)
            {
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

                await EnsurePatientExists(model.PatientId);
                
                var result = _noteService.Create(model);
            
                return CreatedAtAction(nameof(GetById), new { noteId = result.Id }, result);
            }
            
            /// <summary>
            /// Updates an existing <see cref="Note"/> entity. 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="model"></param>
            /// <returns></returns>
            [HttpPut("note/{id}")]
            public async Task<IActionResult> Put(string id, NoteInputModel model)
            {
                if (string.IsNullOrWhiteSpace(id) || model == null)
                {
                    return BadRequest();
                }

                var entity = await _noteService.GetByIdAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                await EnsurePatientExists(model.PatientId);
                await _noteService.Update(entity, model);

                return NoContent();
            }

            /// <summary>
            /// Call the DemographicsAPI to ensure that the Patient entity exists.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            private static async Task EnsurePatientExists(int id)
            {
                using var client = new HttpClient();
                
                var response = await client.GetAsync($"http://demographics_api:80/api/Patient/Exists/{id}");

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var patientExists = JsonConvert.DeserializeObject<bool>(responseBody);

                if (!patientExists)
                {
                    throw new Exception("Error: unable to verify that the specified Patient entity exists.");
                }
            }
        }
}