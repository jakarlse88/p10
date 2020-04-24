using System.Collections.Generic;
using System.Linq;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abarnathy.DemographicsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly DemographicsDbContext _context;
        private readonly IMapper _mapper;

        public PatientController(DemographicsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<PatientInputModel> Get()
        {
            var patients = _context.Patient.ToList();

            return _mapper.Map<IEnumerable<PatientInputModel>>(patients);
        }
    }
}
