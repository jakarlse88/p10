using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public interface IPatientRepository : IRepositoryBase<Patient>
    {
        Task<Patient> GetById(int id);
        Task<IEnumerable<Patient>> GetAll();
    }
}