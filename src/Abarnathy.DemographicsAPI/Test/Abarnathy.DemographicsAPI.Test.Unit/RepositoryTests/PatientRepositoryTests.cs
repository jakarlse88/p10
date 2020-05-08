using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class PatientRepositoryTests
    {
        /**
         * ===============================================================
         * GetAll()
         * ===============================================================
         */
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repository = new PatientRepository(mockContext.Object);

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Patient>>(result);
        }

        /**
         * ===============================================================
         * GetById()
         * ===============================================================
         */
        [Fact]
        public async Task TestGetByIdArgumentBad()
        {
            // Arrange
            var repository = new PatientRepository(null);

            // Act
            async Task<Patient> Action() => await repository.GetById(0);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Action);
        }

        [Fact]
        public async Task TestGetByIdArgumentInvalid()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repository = new PatientRepository(mockContext.Object);

            // Act
            var result = await repository.GetById(666);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetByIdArgumentValid()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repository = new PatientRepository(mockContext.Object);

            // Act
            var result = await repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.IsAssignableFrom<Patient>(result);
        }

        /**
         * ===============================================================
         * GetByFullPersonalia()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestGetByFullPersonaliaModelNull()
        {
            // Arrange
            var repository = new PatientRepository(null);

            // Act
            async Task Action() => await repository.GetByFullPersonalia(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task TestGetByFullPersonaliaModelValid()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var testModel = new PatientInputModel
            {
                FamilyName = "Test",
                GivenName = "Two",
                SexId = 2
            };

            var repository = new PatientRepository(mockContext.Object);

            // Act
            var result = await repository.GetByFullPersonalia(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testModel.FamilyName, result.FamilyName);
            Assert.Equal(testModel.GivenName, result.GivenName);
            Assert.Equal(testModel.SexId, result.SexId);
        }

        [Fact]
        public async Task TestGetByFullPersonaliaModelInvalid()
        {
            // Arrange
            var patients = GeneratePatientEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var testModel = new PatientInputModel
            {
                FamilyName = "Test",
                GivenName = "Twenty",
                SexId = 2
            };

            var repository = new PatientRepository(mockContext.Object);

            // Act
            var result = await repository.GetByFullPersonalia(testModel);

            // Assert
            Assert.Null(result);
        }

        /**
         * ===============================================================
         * Create()
         * ===============================================================
         */

        [Fact]
        public void TestCreateEntityNull()
        {
            // Arrange
            var repository = new PatientRepository(null);

            // Act
            Patient Action() => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Fact]
        public void TestCreateEntityFamilyNameNull()
        {
            // Arrange
            var repository = new PatientRepository(null);

            var testObject = new Patient();

            // Act
            Patient Action() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Fact]
        public void TestCreateEntityGivenNameNull()
        {
            // Arrange
            var repository = new PatientRepository(null);

            var testObject = new Patient
            {
                FamilyName = "Test"
            };

            // Act
            Patient Action() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void TestCreateEntitySexIdOutOfRange(int testId)
        {
            // Arrange
            var repository = new PatientRepository(null);

            var testObject = new Patient
            {
                FamilyName = "Test",
                GivenName = "Test",
                SexId = testId
            };

            // Act
            Patient Action() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Action);
        }

        [Fact]
        public void TestCreateEntityValid()
        {
            // Arrange
            var mockContext = new Mock<DemographicsDbContext>();
            
            mockContext
                .Setup(x => x.Set<Patient>().Add(It.IsAny<Patient>()))
                .Verifiable();
            
            var repository = new PatientRepository(mockContext.Object);

            var testObject = new Patient
            {
                FamilyName = "Test",
                GivenName = "Test",
                SexId = 2
            };

            // Act
            var result = repository.Create(testObject);

            // Assert
            Assert.NotNull(result);

            mockContext
                .Verify(x => x.Set<Patient>().Add(It.IsAny<Patient>()), Times.Once);
        }
        
        /**
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         * Internal helper methods
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         */
        
        private static IEnumerable<Patient> GeneratePatientEntityList()
        {
            var list = new List<Patient>
            {
                new Patient { Id = 1, GivenName = "One", FamilyName = "Test", SexId = 1 },
                new Patient { Id = 2, GivenName = "Two", FamilyName = "Test", SexId = 2 },
                new Patient { Id = 3, GivenName = "Three", FamilyName = "Test", SexId = 1 },
                new Patient { Id = 4, GivenName = "Four", FamilyName = "Test", SexId = 2 },
                new Patient { Id = 5, GivenName = "Five", FamilyName = "Test", SexId = 1 },
            };

            return list;
        }
    }
}