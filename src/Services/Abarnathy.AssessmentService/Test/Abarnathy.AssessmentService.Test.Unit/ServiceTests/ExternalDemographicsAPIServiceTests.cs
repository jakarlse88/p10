using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Abarnathy.DemographicsService.Models;
using Abarnathy.HistoryService.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Abarnathy.AssessmentService.Test.Unit.ServiceTests
{
    public class ExternalDemographicsAPIServiceTests
    {
        [Fact]
        public async Task TestGetPatient()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new Patient()))
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://demographics-service:80")
            };

            var expectedUri = new Uri("http://demographics-service:80/api/patient/1");
            
            var service = new ExternalDemographicsAPIService(httpClient);

            // Act
            var result = await service.GetPatientAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PatientModel>(result);
            
            mockHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

    }
}