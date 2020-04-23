using System;
using System.Collections.Generic;
using System.Linq;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abarnathy.DemographicsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly DemographicsDbContext _context;

        public PatientController(DemographicsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Patient> Get()
        {
            return _context.Patient.ToList();
        }
    }
}
