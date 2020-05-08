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
    public class PhoneNumberRepositoryTests
    {
        /**
         * ===============================================================
         * GetByNumber()
         * ===============================================================
         */
        
        [Fact]
        public async Task TestGetByNumberInputMatches()
        {
            // Arrange
            var phoneNumbers = GeneratePhoneNumberEntityList();

            var mockDbSet = phoneNumbers.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<PhoneNumber>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var testModel = new PhoneNumberInputModel
            {
                Number = "1234567890"
            };

            var repository = new PhoneNumberRepository(mockContext.Object);

            // Act
            var result = await repository.GetByNumber(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1234567890", result.Number);
        }

        [Fact]
        public void TestGetByNumberModelNull()
        {
            // Arrange
            var repository = new PhoneNumberRepository(null);

            // Act
            async Task<PhoneNumber> TestAction() => await repository.GetByNumber(null);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestGetNumberModelNumberNull()
        {
            // Arrange
            var repository = new PhoneNumberRepository(null);

            var testObject = new PhoneNumberInputModel
            {
                Number = null
            };

            // Act
            async Task<PhoneNumber> TestAction() => await repository.GetByNumber(testObject);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestGetNumberModelNumberEmpty()
        {
            // Arrange
            var repository = new PhoneNumberRepository(null);

            var testObject = new PhoneNumberInputModel
            {
                Number = ""
            };

            // Act
            async Task<PhoneNumber> TestAction() => await repository.GetByNumber(testObject);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(TestAction);
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
            var repository = new PhoneNumberRepository(null);

            // Act
            PhoneNumber TestAction() => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityNumberNull()
        {
            // Arrange
            var repository = new PhoneNumberRepository(null);

            var testObject = new PhoneNumber
            {
                Number = null
            };

            // Act
            PhoneNumber TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityNumberEmpty()
        {
            // Arrange
            var repository = new PhoneNumberRepository(null);

            var testObject = new PhoneNumber
            {
                Number = ""
            };

            // Act
            PhoneNumber TestAction() => repository.Create(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestCreateEntityValid()
        {
            // Arrange
            var mockContext = new Mock<DemographicsDbContext>();
            
            mockContext
                .Setup(x => x.Set<PhoneNumber>().Add(It.IsAny<PhoneNumber>()))
                .Verifiable();
            
            var repository = new PhoneNumberRepository(mockContext.Object);

            var testObject = new PhoneNumber
            {
                Number = "1234567890"
            };
            // Act
            var result = repository.Create(testObject);

            // Assert
            Assert.NotNull(result);

            mockContext
                .Verify(x => x.Set<PhoneNumber>().Add(It.IsAny<PhoneNumber>()), Times.Once);
        }


        /**
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         * Internal helper methods
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         */
        private static IEnumerable<PhoneNumber> GeneratePhoneNumberEntityList()
        {
            var list = new List<PhoneNumber>
            {
                new PhoneNumber { Id = 1, Number = "1234567890" },
                new PhoneNumber { Id = 2, Number = "0987654321" }
            };

            return list;
        }
    }
}