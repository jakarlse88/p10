using System.Linq;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Abarnathy.DemographicsAPI.Test.Unit.RepositoryTests
{
    public static class RepositoryTestUtilities 
    {
        public static Mock<DbSet<T>> GenerateMockDbSet<T>(IQueryable<T> entityTQueryable)
            where T : EntityBase
        {
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet
                .As<IQueryable<T>>().Setup(x => x.Provider)
                .Returns(entityTQueryable.Provider);

            mockDbSet
                .As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(entityTQueryable.Expression);

            mockDbSet
                .As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(entityTQueryable.ElementType);

            mockDbSet
                .As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(entityTQueryable.GetEnumerator());
            
            return mockDbSet;
        }
    }
    
    
}