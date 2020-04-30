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
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            var result = 
                await 
                    base.GetByCondition(p => p.Id == id)
                        .Include(p => p.PatientAddresses)
                        .ThenInclude(pa => pa.Address)
                        .Include(p => p.Sex)
                        .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            var result =
                await base.GetByCondition(p => true)
                    .Include(p => p.PatientAddresses)
                    .ThenInclude(pa => pa.Address)
                    .Include(p => p.Sex)
                    .ToListAsync();

            return result;
        }

        public async Task<Patient> GetByFullPersonalia(PatientDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            var result =
                await base.GetByCondition(p => 
                p.FamilyName.Contains(dto.FamilyName) && 
                p.GivenName.Contains(dto.GivenName))
                .FirstOrDefaultAsync(p => p.SexId == dto.SexId && p.DateOfBirth.Date == dto.DateOfBirth.Date);

            return result;
        }
    }
}