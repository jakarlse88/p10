using System;
using System.Collections.Generic;
using System.Linq;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Abarnathy.DemographicsAPI.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public class RepositoryBaseTests
    {
        [Fact]
        public void TestGetByConditionPredicateNull()
        {
            // Arrange
            var repositoryBase = new RepositoryBase<EntityBase>(null);

            // Act
            void TestAction() => repositoryBase.GetByCondition(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestGetByConditionPredicateValidGetAll()
        {
            // Arrange
            var users = GenerateEntityBaseList();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();
            
            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<EntityBase>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repositoryBase =
                new RepositoryBase<EntityBase>(mockContext.Object);

            // Act
            var result = repositoryBase.GetByCondition(_ => true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.ToList().Count);
            Assert.IsAssignableFrom<IQueryable<EntityBase>>(result);
        }


        [Fact]
        public void TestGetByConditionPredicateValidGetSubset()
        {
            // Arrange
            var users = GenerateEntityBaseList();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<EntityBase>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repositoryBase =
                new RepositoryBase<EntityBase>(mockContext.Object);

            // Act
            var result = repositoryBase.GetByCondition(e => e.Id >= 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.ToList().Count);
            Assert.IsAssignableFrom<IQueryable<EntityBase>>(result);
            Assert.DoesNotContain(result.ToList(), e => e.Id < 3);
        }

        [Fact]
        public void TestInsertEntityNull()
        {
            // Arrange
            var repositoryBase = new RepositoryBase<EntityBase>(null);

            // Act
            void TestAction() => repositoryBase.Create(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestInsertEntityNotNull()
        {
            // Arrange
            var testObject = new EntityBase();

            var users = GenerateEntityBaseList();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<EntityBase>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repositoryBase =
                new RepositoryBase<EntityBase>(mockContext.Object);

            // Act
            repositoryBase.Create(testObject);

            // Assert
            mockContext
                .Verify(
                    x => x.Set<EntityBase>().Add(It.IsAny<EntityBase>()),
                    Times.Once);
        }

        [Fact]
        public void TestUpdateEntityNull()
        {
            // Arrange
            var repositoryBase = new RepositoryBase<EntityBase>(null);

            // Act
            void TestAction() => repositoryBase.Update(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestUpdateEntityNotNull()
        {
            // Arrange
            var testObject = new EntityBase { Id = 6 };

            var users = GenerateEntityBaseList();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<EntityBase>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repositoryBase =
                new RepositoryBase<EntityBase>(mockContext.Object);

            // Act
            repositoryBase.Update(testObject);

            // Assert
            mockContext
                .Verify(
                    x => x.Set<EntityBase>().Update(It.IsAny<EntityBase>()),
                    Times.Once);
        }

        [Fact]
        public void TestDeleteEntityNull()
        {
            // Arrange
            var repositoryBase = new RepositoryBase<EntityBase>(null);

            // Act
            void TestAction() => repositoryBase.Delete(null);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestDeleteEntityNotNull()
        {
            // Arrange
            var testObject = new EntityBase { Id = 5 };

            var users = GenerateEntityBaseList();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DemographicsDbContext>();
            mockContext
                .Setup(x => x.Set<EntityBase>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            var repositoryBase =
                new RepositoryBase<EntityBase>(mockContext.Object);

            // Act
            repositoryBase.Delete(testObject);

            // Assert
            mockContext
                .Verify(
                    x => x.Set<EntityBase>().Remove(It.IsAny<EntityBase>()),
                    Times.Once);
        }

        /**
         * Internal helper methods
         * 
         */
        private static IEnumerable<EntityBase> GenerateEntityBaseList()
        {
            return new List<EntityBase>
            {
                new EntityBase { Id = 1 },
                new EntityBase { Id = 2 },
                new EntityBase { Id = 3 },
                new EntityBase { Id = 4 },
                new EntityBase { Id = 5 }
            };
        }
    }
}