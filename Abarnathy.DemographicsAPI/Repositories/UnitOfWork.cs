using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DemographicsDbContext _context;
        private Repository<Address> _addressRepository;
        private Repository<Sex> _sexRepository;
        private Repository<PatientAddress> _patientAddressRepository;
        private Repository<Patient> _patientRepository;
        

        public UnitOfWork(DemographicsDbContext context)
        {
            _context = context;
        }

        public Repository<Patient> PatientRepository =>
            _patientRepository ??= new Repository<Patient>(_context);

        public Repository<Address> AddressRepository =>
            _addressRepository ??= new Repository<Address>(_context);

        public Repository<Sex> SexRepository =>
            _sexRepository ??= new Repository<Sex>(_context);

        public Repository<PatientAddress> PatientAddressRepository =>
            _patientAddressRepository ??= new Repository<PatientAddress>(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public async Task RollbackAsync() =>
            await _context.DisposeAsync();
    }
}