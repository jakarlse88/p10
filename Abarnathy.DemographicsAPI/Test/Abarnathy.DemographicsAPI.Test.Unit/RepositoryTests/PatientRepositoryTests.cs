using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class PatientRepositoryTests
    {
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

        [Fact]
        public async Task TestGetByIdArgumentBad()
        {
            // Arrange
            var repository = new PatientRepository(null);

            // Act
            async Task<Patient> TestAction() => await repository.GetById(0); 

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(TestAction);
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
         * Internal helper methods
         * 
         */
        private static IEnumerable<Patient> GeneratePatientEntityList()
        {
            // var address = new Address
            // {
            //     Id = 1,
            //     Town = "TestTown"
            // };
            //
            // var patient = new Patient
            // {
            //     FamilyName = "One",
            //     GivenName = "Two",
            //     Sex = new Sex { Id = 1 },
            //     PatientAddress = new List<PatientAddress>(),
            //     PhoneNumber = ""
            // };
            //
            // var patientAddress = new PatientAddress
            // {
            //     PatientId = 1,
            //     Patient = patient,
            //     AddressId = 1,
            //     Address = address
            // };
            //
            // patient.PatientAddress.Add(patientAddress);
            //
            // var list = new List<Patient> { patient };

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