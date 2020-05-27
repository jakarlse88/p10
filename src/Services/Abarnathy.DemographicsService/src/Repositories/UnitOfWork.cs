using System.Threading.Tasks;
using Abarnathy.DemographicsService.Data;

namespace Abarnathy.DemographicsService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DemographicsDbContext _context;
        private IPatientRepository _patientRepository;
        private IAddressRepository _addressRepository;
        private IPhoneNumberRepository _phoneNumberRepository;

        public UnitOfWork(DemographicsDbContext context)
        {
            _context = context;
        }

        public IPatientRepository PatientRepository =>
            _patientRepository ??= new PatientRepository(_context);

        public IAddressRepository AddressRepository =>
            _addressRepository ??= new AddressRepository(_context);

        public IPhoneNumberRepository PhoneNumberRepository =>
            _phoneNumberRepository ??= new PhoneNumberRepository(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public async Task RollbackAsync() =>
            await _context.DisposeAsync();
    }
}