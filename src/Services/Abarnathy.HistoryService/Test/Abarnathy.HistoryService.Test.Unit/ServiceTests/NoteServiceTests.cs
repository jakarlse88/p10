using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryService.Infrastructure;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;
using Abarnathy.HistoryService.Repositories;
using Abarnathy.HistoryService.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace Abarnathy.HistoryAPI.Test.ServiceTests
{
    public class NoteServiceTests
    {
        private readonly IMapper _mapper;

        public NoteServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new MappingProfiles()); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task TestGetByPatientIdAsInputModelNoResults()
        {
            // Arrange
            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Note>
                {
                    new Note(),
                    new Note(),
                    new Note()
                });

            var service = new NoteService(null, mockRepository.Object);

            // Act
            var result = await service.GetByPatientIdAsync(2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGetByPatientIdResults()
        {
            // Arrange
            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Note>
                {
                    new Note(),
                    new Note(),
                    new Note()
                });

            var service = new NoteService(null, mockRepository.Object);

            // Act
            var result = await service.GetByPatientIdAsync(1);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task TestGetByIdAsyncIdBad(string testId)
        {
            // Arrange
            var service = new NoteService(null, null);

            // Act
            async Task<Note> TestAction() => await service.GetByIdAsync(testId);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestGetByIdIdInvalid()
        {
            // Arrange
            var testId = "testId";

            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.GetByIdAsync(testId))
                .ReturnsAsync(new Note());

            var service = new NoteService(null, mockRepository.Object);

            // Act
            var result = await service.GetByIdAsync("otherId");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetByIdIdValid()
        {
            // Arrange
            var testId = "testId";

            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.GetByIdAsync(testId))
                .ReturnsAsync(new Note
                {
                    Title = "Test"
                });

            var service = new NoteService(null, mockRepository.Object);

            // Act
            var result = await service.GetByIdAsync(testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public async Task TestCreateModelNull()
        {
            // Arrange
            var service = new NoteService(null, null);

            // Act
            async Task<Note> TestAction() => await service.Create(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestCreateModelNotNull()
        {
            // Arrange
            var testModel = new NoteCreateModel
            {
                Title = "Test"
            };

            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.Insert(It.IsAny<Note>()))
                .Verifiable();

            var service = new NoteService(_mapper, mockRepository.Object);

            // Act
            var result = await service.Create(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);

            mockRepository
                .Verify(x => x.Insert(It.IsAny<Note>()), Times.Once());
        }

        [Fact]
        public async Task TestUpdateEntityNull()
        {
            // Arrange
            var service = new NoteService(null, null);

            // Act
            async Task TestAction() => await service.Update(null, null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("entity", ex.ParamName);
        }

        [Fact]
        public async Task TestUpdateModelNull()
        {
            // Arrange
            var service = new NoteService(null, null);

            // Act
            async Task TestAction() => await service.Update(new Note(), null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("model", ex.ParamName);
        }

        [Fact]
        public async Task TestUpdateModelValid()
        {
            // Arrange
            var testEntity = new Note
            {
                Id = Guid.NewGuid().ToString()
            };

            var testModel = new NoteInputModel
            {
                Title = "Test"
            };

            var mockRepository = new Mock<INoteRepository>();
            mockRepository
                .Setup(x => x.Update(testEntity.Id, It.IsAny<Note>()))
                .ReturnsAsync((string id, Note note) => new Note
                {
                    Id = id,
                    Title = note.Title
                })
                .Verifiable();

            var service = new NoteService(_mapper, mockRepository.Object);

            // Act
            var result = await service.Update(testEntity, testModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
            mockRepository
                .Verify(
                    x => x.Update(It.IsAny<string>(), It.IsAny<Note>()),
                    Times.Once());
        }
    }
}