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
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetInputModelsAll())
                .ReturnsAsync(new List<PatientDTO> { new PatientDTO { Id = 1 } })
                .Verifiable();

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var objectResult =
                Assert.IsAssignableFrom<IEnumerable<PatientDTO>>(actionResult.Value);

            mockService
                .Verify(x => x.GetInputModelsAll(), Times.Once());
        }

        [Fact]
        public async Task TestGetIdBad()
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.Get(666);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestGetIdValid()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetInputModelById(5))
                .ReturnsAsync(new PatientDTO { Id = 5 });

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get(5);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var objectResult =
                Assert.IsAssignableFrom<PatientDTO>(actionResult.Value);

            mockService
                .Verify(x => x.GetInputModelById(5), Times.Once);
        }

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
                .Setup(x => x.Create(It.IsAny<PatientDTO>()))
                .ReturnsAsync(4);

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Post(new PatientDTO());

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<CreatedAtActionResult>(result);

            Assert.Equal("Get", actionResult.ActionName);
        }
    }
}