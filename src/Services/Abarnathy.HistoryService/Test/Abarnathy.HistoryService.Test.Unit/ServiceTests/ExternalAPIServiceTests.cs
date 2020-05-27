using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using Xunit;

namespace Abarnathy.HistoryAPI.Test.ServiceTests
{
    public class ExternalAPIServiceTests
    {
        [Fact]
        public async Task TestPatientExistsIdValid()
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
                    StatusCode = HttpStatusCode.NoContent
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://demographics_api:80")
            };

            var service = new ExternalAPIService(httpClient);

            var expectedUri = new Uri("http://demographics_api:80/api/Patient/Exists/1");

            // Act
            var result = await service.PatientExists(1);

            // Assert
            Assert.True(result);
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

        [Fact]
        public async Task TestPatientExistsIdInvalid()
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
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://demographics_api:80")
            };

            var service = new ExternalAPIService(httpClient);

            var expectedUri = new Uri("http://demographics_api:80/api/Patient/Exists/1");

            // Act
            var result = await service.PatientExists(1);

            // Assert
            Assert.False(result);
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