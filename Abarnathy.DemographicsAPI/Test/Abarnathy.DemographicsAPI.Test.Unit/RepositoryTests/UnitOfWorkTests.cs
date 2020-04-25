using System.Threading;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Repositories;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class UnitOfWorkTests
    {
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