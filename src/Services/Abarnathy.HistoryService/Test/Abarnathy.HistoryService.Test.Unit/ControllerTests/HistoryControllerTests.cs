using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Controllers;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;
using Abarnathy.HistoryService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Abarnathy.HistoryAPI.Test.ControllerTests
{
    public class HistoryControllerTests
    {
        /**
         * ==============================================
         * GetByNoteId()
         * ==============================================
         */
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task TestGetByIdIdBad(string testId)
        {
            // Arrange
            var controller = new HistoryController(null, null);

            // Act
            var result = await controller.GetByNoteId(testId);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task TestGetBydIdIdInvalid()
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(null as Note);


            var controller = new HistoryController(mockService.Object, null);

            // Act
            var result = await controller.GetByNoteId("test");

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task TestGetByIdIdValid()
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Note());

            var controller = new HistoryController(mockService.Object, null);

            // Act
            var result = await controller.GetByNoteId("test");

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            Assert.IsAssignableFrom<NoteInputModel>(actionResult.Value);
        }

        /**
         * ==============================================
         * GetByPatientId()
         * ==============================================
         */
        [Fact]
        public async Task TestGetByPatientIdPatientIdInvalid()
        {
            // Arrange
            var mockService = new Mock<IExternalAPIService>();
            mockService
                .Setup(x => x.PatientExists(It.IsAny<int>()))
                .ReturnsAsync(false);

            var controller = new HistoryController(null, mockService.Object);

            // Act
            var result = await controller.GetByPatientId(1);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task TestGetByPatientIdPatientIdValidNoNotes()
        {
            // Arrange
            var mockNoteService = new Mock<INoteService>();
            mockNoteService
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Note>());

            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(true);

            var controller = new HistoryController(mockNoteService.Object, mockExternalService.Object);

            // Act
            var result = await controller.GetByPatientId(1);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task TestGetByPatientIdPatientIdValid()
        {
            // Arrange
            var mockNoteService = new Mock<INoteService>();
            mockNoteService
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Note>
                {
                    new Note(),
                    new Note(),
                    new Note()
                });

            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(true);

            var controller = new HistoryController(mockNoteService.Object, mockExternalService.Object);

            // Act
            var result = await controller.GetByPatientId(1);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var objectResult =
                Assert.IsAssignableFrom<IEnumerable<NoteInputModel>>(actionResult.Value);
            Assert.Equal(3, objectResult.Count());
        }

        /**
         * ==============================================
         * Post()
         * ==============================================
         */
        [Fact]
        public async Task TestPostModelNull()
        {
            // Arrange
            var controller = new HistoryController(null, null);

            // Act
            var result = await controller.Post(null);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task TestPostModelNotNull()
        {
            // Arrange
            var testModel = new NoteCreateModel { PatientId = 1 };
            var testNoteId = Guid.NewGuid().ToString();

            var mockNoteService = new Mock<INoteService>();
            mockNoteService
                .Setup(x => x.Create(testModel))
                .ReturnsAsync((NoteCreateModel note) =>
                new Note
                {
                    Id = testNoteId,
                    PatientId = note.PatientId
                });

            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(true);

            var controller = new HistoryController(mockNoteService.Object, mockExternalService.Object);

            // Act
            var result = await controller.Post(testModel);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);

            Assert.Equal("GetByNoteId", actionResult.ActionName);
            Assert.Equal(testNoteId, actionResult.RouteValues.FirstOrDefault().Value);

            var modelResult =
                Assert.IsAssignableFrom<Note>(actionResult.Value);
        }

        /**
         * ==============================================
         * Put()
         * ==============================================
         */
        [Fact]
        public async Task TestPutIdBad()
        {
            // Arrange
            var controller = new HistoryController(null, null);

            // Act
            var result = await controller.Put(null, null);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestPutModelNull()
        {
            // Arrange
            var controller = new HistoryController(null, null);

            // Act
            var result = await controller.Put("abc", null);

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestPutPatientDoesNotExist()
        {
            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(false);

            var controller =
                new HistoryController(null, mockExternalService.Object);

            // Act
            var result = await controller.Put("abc", new NoteInputModel
            {
                PatientId = 1
            });

            // Assert
            var actionResult = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
            Assert.Equal("Patient not found", actionResult.Value);
        }

        [Fact]
        public async Task TestPutValidEntityNull()
        {
            // Arrange
            var testModel = new NoteInputModel { PatientId = 1 };

            var mockNoteService = new Mock<INoteService>();
            mockNoteService
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(null as Note);

            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(true);

            var controller = new HistoryController(mockNoteService.Object, mockExternalService.Object);

            // Act
            var result =
                await controller.Put("abc", testModel);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task TestPutValid()
        {
            // Arrange
            var testModel = new NoteInputModel { PatientId = 1 };
            var testNoteId = Guid.NewGuid().ToString();

            var mockNoteService = new Mock<INoteService>();
            mockNoteService
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Note { Id = testNoteId });

            mockNoteService
                .Setup(x => x.Update(It.IsAny<Note>(), It.IsAny<NoteInputModel>()))
                .Verifiable();

            var mockExternalService = new Mock<IExternalAPIService>();
            mockExternalService
                .Setup(x => x.PatientExists(1))
                .ReturnsAsync(true);

            var controller = new HistoryController(mockNoteService.Object, mockExternalService.Object);

            // Act
            var result =
                await controller.Put("abc", testModel);

            // Assert
            var actionResult =
                Assert.IsAssignableFrom<NoContentResult>(result);
        }
    }
}