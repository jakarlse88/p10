using System.Threading;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Repositories;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class UnitOfWorkTests
    {
        /**
         * ===============================================================
         * PatientRepository()
         * ===============================================================
         */
        
        [Fact]
        public void TestPatientRepository()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<DemographicsDbContext>());

            // Act
            var patientRepository = unitOfWork.PatientRepository;

            // Assert
            Assert.NotNull(patientRepository);
            Assert.IsAssignableFrom<IPatientRepository>(patientRepository);
        }

        /**
         * ===============================================================
         * AddressRepository()
         * ===============================================================
         */
        
        [Fact]
        public void TestAddressRepository()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<DemographicsDbContext>());

            // Act
            var addressRepository = unitOfWork.AddressRepository;

            // Assert
            Assert.NotNull(addressRepository);
            Assert.IsAssignableFrom<IAddressRepository>(addressRepository);
        }
        
        /**
         * ===============================================================
         * PhoneNumberRepository
         * ===============================================================
         */
        
        [Fact]
        public void TestPhoneNumberRepository()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<DemographicsDbContext>());

            // Act
            var phoneNumberRepository = unitOfWork.PhoneNumberRepository;

            // Assert
            Assert.NotNull(phoneNumberRepository);
            Assert.IsAssignableFrom<IPhoneNumberRepository>(phoneNumberRepository);
        }
        
        /**
         * ===============================================================
         * CommitAsync()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestCommitAsync()
        {
            // Arrange
            var mockContext = new Mock<DemographicsDbContext>();

            mockContext
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            var unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.CommitAsync();

            // Assert
            mockContext
                .Verify(x =>
                    x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        /**
         * ===============================================================
         * RollbackAsync()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestRollbackAsync()
        {
            // Arrange
            var mockContext = new Mock<DemographicsDbContext>();

            mockContext
                .Setup(x => x.DisposeAsync())
                .Verifiable();

            var unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.RollbackAsync();

            // Assert
            mockContext
                .Verify(x =>
                        x.DisposeAsync(),
                    Times.Once
                );
        }
    }
}