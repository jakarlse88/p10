using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Models;
using Abarnathy.DemographicsService.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class AddressRepositoryTests
    {
        /**
         * ===============================================================
         * Create()
         * ===============================================================
         */
        
        [Fact]
        public void TestCreateEntityNull()
        {
            // Arrange
            var repository = new AddressRepository(null);
            
            // Act
            Address TestAction() => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityStreetNameNull()
        {
            // Arrange
            var repository = new AddressRepository(null);

            var testObject = new Address();
            
            // Act
            Address TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityHouseNumberNull()
        {
            // Arrange
            var repository = new AddressRepository(null);

            var testObject = new Address
            {
                StreetName = "asdad"
            };
            
            // Act
            Address TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityTownNull()
        {
            // Arrange
            var repository = new AddressRepository(null);

            var testObject = new Address
            {
                StreetName = "Test",
                HouseNumber = "Test"
            };
            
            // Act
            Address TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityStateNull()
        {
            // Arrange
            var repository = new AddressRepository(null);

            var testObject = new Address
            {
                StreetName = "Test",
                HouseNumber = "Test",
                Town = "Test"
            };
            
            // Act
            Address TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityZipcodeNull()
        {
            // Arrange
            var repository = new AddressRepository(null);

            var testObject = new Address
            {
                StreetName = "Test",
                HouseNumber = "Test",
                Town = "Test",
                State = "Test"
            };
            
            // Act
            Address TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityValid()
        {
            // Arrange
            var mockContext = new Mock<DemographicsDbContext>();
            
            mockContext
                .Setup(x => x.Set<Address>().Add(It.IsAny<Address>()))
                .Verifiable();
            
            var repository = new AddressRepository(mockContext.Object);

            var testObject = new Address
            {
                StreetName = "Test",
                HouseNumber = "Test",
                Town = "Test",
                State = "Test",
                ZipCode = "Test"
            };

            // Act
            var result = repository.Create(testObject);

            // Assert
            Assert.NotNull(result);

            mockContext
                .Verify(x => x.Set<Address>().Add(It.IsAny<Address>()), Times.Once);
        }

        /**
         * ===============================================================
         * GetByCompleteAddressAsync()
         * ===============================================================
         */

        [Fact]
        public async Task TestGetByCompleteAddressAsyncModelValid()
        {
            // Arrange
            var patients = GenerateAddressEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Address>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var testModel = new AddressInputModel
            {
                StreetName = "Test Street",
                HouseNumber = "1",
                Town = "Test Town",
                State = "Test State",
                ZipCode = "12345"
            };

            var repository = new AddressRepository(mockContext.Object);

            // Act
            var result = await repository.GetByCompleteAddressAsync(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testModel.State, result.State);
        }

        [Fact]
        public async Task TestGetByCompleteAddressAsyncModelInvalid()
        {
            // Arrange
            var patients = GenerateAddressEntityList();

            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<Address>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var testModel = new AddressInputModel
            {
                StreetName = "Test Street",
                HouseNumber = "666",
                Town = "Test Town",
                State = "Test State",
                ZipCode = "12345"
            };

            var repository = new AddressRepository(mockContext.Object);

            // Act
            var result = await repository.GetByCompleteAddressAsync(testModel);

            // Assert
            Assert.Null(result);
        }

        /**
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         * Internal helper methods
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         */
        
        private static IEnumerable<Address> GenerateAddressEntityList()
        {
            var list = new List<Address>
            {
                new Address
                {
                    StreetName = "Test Street",
                    HouseNumber = "1",
                    Town = "Test Town",
                    State = "Test State",
                    ZipCode = "12345"
                },
                new Address
                {
                    StreetName = "Test Street",
                    HouseNumber = "2",
                    Town = "Test Town",
                    State = "Test State",
                    ZipCode = "12345"
                },
                new Address
                {
                    StreetName = "Test Street",
                    HouseNumber = "2",
                    Town = "Test Town",
                    State = "Test State",
                    ZipCode = "12345"
                },
            };

            return list;
        }
    }
}