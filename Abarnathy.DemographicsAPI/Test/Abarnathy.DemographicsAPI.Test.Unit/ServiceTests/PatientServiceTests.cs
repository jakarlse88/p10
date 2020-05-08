using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Infrastructure;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using Abarnathy.DemographicsAPI.Services;
using AutoMapper;
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

        /**
         * ===============================================================
         * GetInputModelById()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestGetInputModelByIdArgumentBad()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task<PatientInputModel> Action() => await service.GetInputModelById(0);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Action);
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

        /**
         * ===============================================================
         * GetEntityById()
         * ===============================================================
         */
        [Fact]
        public async Task TestGetByIdArgumentBad()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task<Patient> Action() => await service.GetEntityById(0);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Action);
        }

        [Fact]
        public async Task TestGetEntityByIdArgumentInvalid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetById(5))
                .ReturnsAsync(new Patient { Id = 5 });

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetEntityById(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetEntityByIdArgumentValid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetById(5))
                .ReturnsAsync(new Patient { Id = 5 });

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetEntityById(5);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Patient>(result);
            Assert.Equal(5, result.Id);
        }

        /**
         * ===============================================================
         * GetInputModelsAll()
         * ===============================================================
         */
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

        /**
         * ===============================================================
         * Create()
         * ===============================================================
         */
        [Fact]
        public async Task TestCreateModelNull()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task Action() => await service.Create(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task TestCreateModelValid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Create(It.IsAny<Patient>()))
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
            Assert.IsAssignableFrom<Patient>(result);

            mockUnitOfWork
                .Verify(x => x.PatientRepository.Create(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task TestCreateException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Create(It.IsAny<Patient>()))
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
            async Task<Patient> Action() => await service.Create(new PatientInputModel
            {
                FamilyName = "Test",
            });

            // Assert
            await Assert.ThrowsAsync<Exception>(Action);
            mockUnitOfWork
                .Verify(x => x.PatientRepository.Create(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);

            mockUnitOfWork
                .Verify(x => x.RollbackAsync(), Times.Once());
        }

        [Fact]
        public async Task TestCreatePatientEntityAlreadyExists()
        {
            // Arrange
            var testDTO = new PatientInputModel();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetByFullPersonalia(It.IsAny<PatientInputModel>()))
                .ReturnsAsync(new Patient())
                .Verifiable();

            var service = new PatientService(mockUnitOfWork.Object, null);

            // Act
            async Task Action() => await service.Create(testDTO);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(Action);
            Assert.Equal("Error: a Patient entity already exists that matches the supplied personalia.", ex.Message);
        }

        [Fact]
        public async Task TestCreateDTOAddressesNull()
        {
            // Arrange
            var testDTO = new PatientInputModel { Addresses = null };

            var service = new PatientService(null, null);

            // Act
            async Task Action() => await service.Create(testDTO);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task TestCreateDTOPhoneNumbersNull()
        {
            // Arrange
            var testDTO = new PatientInputModel { PhoneNumbers = null };

            var service = new PatientService(null, null);

            // Act
            async Task Action() => await service.Create(testDTO);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        /**
         * ===============================================================
         * Update()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestUpdateEntityNull()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task Action() => await service.Update(null, null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task TestUpdateModelNull()
        {
            // Arrange
            var service = new PatientService(null, null);

            // Act
            async Task Action() => await service.Update(new Patient(), null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task TestUpdateArgumentsValid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Update(It.IsAny<Patient>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Verifiable();

            var service = new PatientService(mockUnitOfWork.Object, _mapper);

            // Act
            await service.Update(new Patient { FamilyName = "Tset" }, new PatientInputModel
            {
                FamilyName = "Test"
            });

            // Assert
            mockUnitOfWork
                .Verify(x => x.PatientRepository.Update(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task TestUpdateException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PatientRepository.Update(It.IsAny<Patient>()))
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
            async Task Action() => 
                await service.Update(
                    new Patient { FamilyName = "Tset" }, 
                    new PatientInputModel { FamilyName = "Test" }
                    );

            // Assert
            await Assert.ThrowsAsync<Exception>(Action);
            
            mockUnitOfWork
                .Verify(x => x.PatientRepository.Update(It.IsAny<Patient>()), Times.Once);

            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);

            mockUnitOfWork
                .Verify(x => x.RollbackAsync(), Times.Once());
        }


        /**
         * ===============================================================
         * Internal helper methods
         * ===============================================================
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

        private static IEnumerable<Address> GenerateAddressEntityList()
        {
            var list = new List<Address>
            {
                new Address
                {
                    StreetName = "Main Street",
                    HouseNumber = "123",
                    Town = "Townsville",
                    State = "Washington",
                    ZipCode = "12345"
                },
                new Address
                {
                    StreetName = "Back Street",
                    HouseNumber = "123",
                    Town = "Townsville",
                    State = "Washington",
                    ZipCode = "12345"
                }
            };

            return list;
        }
        
        private static ICollection<AddressInputModel> GenerateAddressInputModelList()
        {
            var list = new List<AddressInputModel>
            {
                new AddressInputModel
                {
                    StreetName = "Main Street",
                    HouseNumber = "123",
                    Town = "Townsville",
                    State = "Washington",
                    ZipCode = "12345"
                },
                new AddressInputModel
                {
                    StreetName = "Back Street",
                    HouseNumber = "123",
                    Town = "Townsville",
                    State = "Washington",
                    ZipCode = "12345"
                }
            };

            return list;
        }

        private static IEnumerable<PhoneNumber> GeneratePhoneNumberEntityList()
        {
            var list = new List<PhoneNumber>
            {
                new PhoneNumber
                {
                    Number = "1234567890"
                },
                new PhoneNumber
                {
                    Number = "0987654321"
                }
            };

            return list;
        }
        
        private static ICollection<PhoneNumberInputModel> GeneratePhoneNumberInputModelList()
        {
            var list = new List<PhoneNumberInputModel>
            {
                new PhoneNumberInputModel
                {
                    Number = "1234567890"
                },
                new PhoneNumberInputModel
                {
                    Number = "0987654321"
                }
            };

            return list;
        }

    }
}