using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public PatientRepository(DemographicsDbContext context) : base(context)
        {
        }

        public async Task<Patient> GetById(int id)
        {
            var result = 
                await 
                    base.GetByCondition(p => p.Id == id)
                        .Include(p => p.PatientAddress)
                        .ThenInclude(pa => pa.Address)
                        .Include(p => p.Sex)
                        .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            var result =
                await base.GetByCondition(_ => true)
                    .Include(p => p.PatientAddress)
                    .ThenInclude(pa => pa.Address)
                    .Include(p => p.Sex)
                    .ToListAsync();

            return result;
        }
    }
}