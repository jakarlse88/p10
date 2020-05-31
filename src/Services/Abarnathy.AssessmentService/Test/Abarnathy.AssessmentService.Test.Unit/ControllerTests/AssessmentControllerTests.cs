using System.Threading.Tasks;
using Abarnathy.AssessmentService.Controllers;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Abarnathy.AssessmentService.Test.Unit.ControllerTests
{
    public class AssessmentControllerTests
    {
        [Fact]
        public async Task TestGetPatientNull()
        {
            // Arrange
            var mockService = new Mock<IExternalDemographicsAPIService>();
            mockService
                .Setup(x => x.GetPatientAsync(1))
                .ReturnsAsync(null as PatientModel);

            var controller = new AssessmentController(mockService.Object, null);
            
            // Act
            var result = await controller.Get(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
            Assert.Equal("Patient not found.", actionResult.Value);
        }

        [Fact]
        public async Task TestGetPatientFound()
        {
            // Arrange
            var mockExternalService = new Mock<IExternalDemographicsAPIService>();
            mockExternalService
                .Setup(x => x.GetPatientAsync(1))
                .ReturnsAsync(new PatientModel());

            var mockAssessmentService = new Mock<IAssessmentService>();
            mockAssessmentService
                .Setup(x => x.GenerateAssessment(It.IsAny<PatientModel>()))
                .ReturnsAsync(new AssessmentResult(1, RiskLevel.None));
            
            var controller = new AssessmentController(mockExternalService.Object, mockAssessmentService.Object);
            
            // Act
            var result = await controller.Get(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var modelResult = Assert.IsAssignableFrom<AssessmentResult>(actionResult.Value);
            Assert.Equal(RiskLevel.None, modelResult.RiskLevel);
        }

    }
}