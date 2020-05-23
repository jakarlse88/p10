using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.HistoryAPI.Data;
using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Repositories;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Abarnathy.HistoryAPI.Test.RepositoryTests
{
    public class NoteRepositoryTests : IDisposable
    {
        private static MongoDbRunner _runner;
        private static MongoClient _client;
        private static Note[] _testModels;

        public NoteRepositoryTests()
        {
            _testModels = new[]
            {
                new Note
                {
                    Title = "1",
                    PatientId = 1
                },
                new Note
                {
                    Title = "2",
                    PatientId = 1
                },
                new Note
                {
                    Title = "3",
                    PatientId = 2
                }
            };

            _runner = MongoDbRunner.Start();

            _client = new MongoClient(_runner.ConnectionString);

            var db = _client.GetDatabase("IntegrationTests");

            var collection = db.GetCollection<Note>("Notes");

            collection.InsertMany(_testModels);
        }

        [Fact]
        public async Task TestGetByIdAsyncIdValid()
        {
            // Arrange
            var repository = new NoteRepository(new PatientHistoryDbContext(_client, "IntegrationTests"));

            // Act
            var result = await repository.GetByIdAsync(_testModels[0].Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Title);
        }

        [Fact]
        public async Task TestGetByPatientIdExists()
        {
            // Arrange
            var repository = new NoteRepository(new PatientHistoryDbContext(_client, "IntegrationTests"));

            // Act
            var result = await repository.GetByPatientIdAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Note>>(result);
        }

        [Fact]
        public async Task TestGetByPatientIdDoesNotExist()
        {
            // Arrange
            var repository = new NoteRepository(new PatientHistoryDbContext(_client, "IntegrationTests"));

            // Act
            var result = await repository.GetByPatientIdAsync(3);

            // Assert
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<Note>>(result);
        }

        [Fact]
        public async Task TestInsertEntityNull()
        {
            // Arrange
            var repository = new NoteRepository(null);

            // Act
            async Task TestAction() => await repository.Insert(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("entity", ex.ParamName);
        }

        [Fact]
        public async Task TestInsertEntityNotNull()
        {
            // Arrange
            var repository = new NoteRepository(new PatientHistoryDbContext(_client, "IntegrationTests"));

            Assert.Equal(3,
                await _client.GetDatabase("IntegrationTests").GetCollection<Note>("Notes")
                    .CountDocumentsAsync(_ => true));

            // Act
            await repository.Insert(new Note { Title = "Insert" });

            // Assert
            Assert.Equal(4,
                await _client.GetDatabase("IntegrationTests").GetCollection<Note>("Notes")
                    .CountDocumentsAsync(_ => true));
        }

        [Fact]
        public async Task TestUpdateIdNull()
        {
            // Arrange
            var repository = new NoteRepository(null);

            // Act
            async Task TestAction() => await repository.Update(null, null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("id", ex.ParamName);
        }

        [Fact]
        public async Task TestUpdateBookInNull()
        {
            // Arrange
            var repository = new NoteRepository(null);

            // Act
            async Task TestAction() => await repository.Update("abc", null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("bookIn", ex.ParamName);
        }

        [Fact]
        public async Task TestUpdateArgumentsValid()
        {
            // Arrange
            var repository = new NoteRepository(new PatientHistoryDbContext(_client, "IntegrationTests"));

            var result = await _client.GetDatabase("IntegrationTests").GetCollection<Note>("Notes")
                .FindAsync(n => n.Id == _testModels[0].Id);

            var note = result.FirstOrDefault();

            Assert.Equal("1", note.Title);

            note.Title = "Updated";

            // Act
            await repository.Update(note.Id, note);

            // Assert
            Assert.Contains(_client.GetDatabase("IntegrationTests").GetCollection<Note>("Notes").AsQueryable(), n => n.Title == "Updated");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task TestGetByIdAsyncIdBad(string testId)
        {
            // Arrange
            var repository = new NoteRepository(null);

            // Act
            async Task<Note> TestAction() => await repository.GetByIdAsync(testId);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("id", ex.ParamName);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}