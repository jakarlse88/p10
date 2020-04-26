using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Infrastructure;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Abarnathy.DemographicsAPI.Services;
using AutoMapper;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.ServiceTests
{
    public class PatientServiceTests
    {
        private readonly IMapper _mapper;

        public PatientServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new MappingProfiles()); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task TestGetInputModelByIdArgumentBad()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task<PatientInputModel> TestAction() => await service.GetInputModelById(0);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(TestAction);
        }

        [Fact]
        public async Task TestGetInputModelByIdArgumentInvalid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetById(5))
                .ReturnsAsync(new Patient { Id = 5 });

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetInputModelById(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetInputModelByIdArgumentValid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetById(5))
                .ReturnsAsync(new Patient { Id = 5 });

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetInputModelById(5);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PatientInputModel>(result);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task TestGetInputModelsAll()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetAll())
                .ReturnsAsync(patients);

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetInputModelsAll();

            // Assert
            Assert.Equal(5, result.Count());
            Assert.IsAssignableFrom<IEnumerable<PatientInputModel>>(result);
        }

        [Fact]
        public async Task TestCreateModelNull()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task TestAction() => await service.Create(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestCreateModelValid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Insert(It.IsAny<Patient>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Verifiable();

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.Create(new PatientInputModel
            {
                FamilyName = "Test",
            });

            // Assert
            mockUnitOfWork
                .Verify(x => x.PatientRepository.Insert(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task TestCreateException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Insert(It.IsAny<Patient>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Throws<Exception>()
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.RollbackAsync())
                .Verifiable();

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            async Task<int> TestAction() => await service.Create(new PatientInputModel
            {
                FamilyName = "Test",
            });

            // Assert
            await Assert.ThrowsAsync<Exception>(TestAction);
            mockUnitOfWork
                .Verify(x => x.PatientRepository.Insert(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);

            mockUnitOfWork
                .Verify(x => x.RollbackAsync(), Times.Once());
        }


        /**
         * Internal helper methods
         * 
         */
        private static IEnumerable<Patient> GeneratePatientEntityList()
        {
            var list = new List<Patient>
            {
                new Patient { Id = 1 },
                new Patient { Id = 2 },
                new Patient { Id = 3 },
                new Patient { Id = 4 },
                new Patient { Id = 5 }
            };

            return list;
        }
    }
}