using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Services;
using MongoDB.Bson.IO;
using Moq;
using Moq.Protected;
using Xunit;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Abarnathy.AssessmentService.Test.Unit.ServiceTests
{
    public class ExternalHistoryAPIServiceTests
    {
        [Fact]
        public async Task TestGetPatientHistory()
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
                    Content = new StringContent(JsonConvert.SerializeObject(new[] { new Note() }))
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://history-service:80")
            };

            var expectedUri = new Uri("http://history-service:80/api/history/patient/1");
            
            var service = new ExternalHistoryAPIService(httpClient);

            // Act
            var result = await service.GetPatientHistoryAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<NoteModel>>(result);
            Assert.Single(result);
            
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