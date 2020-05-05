using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Controllers;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.ControllerTests
{
    public class PatientControllerTests
    {
        /**
         * Get() 
         */
        
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetInputModelsAll())
                .ReturnsAsync(new List<PatientInputModel> { new PatientInputModel { Id = 1 } })
                .Verifiable();

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var objectResult =
                Assert.IsAssignableFrom<IEnumerable<PatientInputModel>>(actionResult.Value);

            mockService
                .Verify(x => x.GetInputModelsAll(), Times.Once());
        }

        /**
         * Get(int id)
         */
        
        [Fact]
        public async Task TestGetIdInvalid()
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.Get(0);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task TestGetIdBad()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetInputModelById(666))
                .ReturnsAsync((PatientInputModel) null);

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get(666);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(result.Result);
        }


        [Fact]
        public async Task TestGetIdValid()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetInputModelById(5))
                .ReturnsAsync(new PatientInputModel { Id = 5 });

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get(5);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var objectResult =
                Assert.IsAssignableFrom<PatientInputModel>(actionResult.Value);

            mockService
                .Verify(x => x.GetInputModelById(5), Times.Once);
        }

        /**
         * Post()
         */

        [Fact]
        public async Task TestPostModelNull()
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.Post(null);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestPostModelNotNull()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.Create(It.IsAny<PatientInputModel>()))
                .ReturnsAsync(new Patient());

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Post(new PatientInputModel());

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<CreatedAtActionResult>(result);

            Assert.Equal("Get", actionResult.ActionName);
            
            var modelResult =
                Assert.IsAssignableFrom<Patient>(actionResult.Value);
        }
        
        /**
         * PUT()
         */

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task TestPutIdBad(int testId)
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.Put(testId, null);
            
            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestPutModelNull()
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.Put(1, null);
            
            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }


    }
}