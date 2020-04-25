using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Services.Interfaces;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DemographicsDbContext _context;
        private RepositoryBase<Address> _addressRepositoryBase;
        private RepositoryBase<Sex> _sexRepositoryBase;
        private RepositoryBase<PatientAddress> _patientAddressRepositoryBase;
        private IPatientRepository _patientRepository;

        public UnitOfWork(DemographicsDbContext context)
        {
            _context = context;
        }

        public IPatientRepository PatientRepository =>
            _patientRepository ??= new PatientRepository(_context);

        public RepositoryBase<Address> AddressRepositoryBase =>
            _addressRepositoryBase ??= new RepositoryBase<Address>(_context);

        public RepositoryBase<Sex> SexRepositoryBase =>
            _sexRepositoryBase ??= new RepositoryBase<Sex>(_context);

        public RepositoryBase<PatientAddress> PatientAddressRepositoryBase =>
            _patientAddressRepositoryBase ??= new RepositoryBase<PatientAddress>(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public async Task RollbackAsync() =>
            await _context.DisposeAsync();
    }
}