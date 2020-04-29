using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public static class SeedData
    {
        public static void PopulateTestData(DemographicsDbContext context)
        {
            context.Patient.AddRange(
                new Patient
                {
                    FamilyName = "Doe",
                    GivenName = "Jane",
                },
                new Patient
                {
                    FamilyName = "Smith",
                    GivenName = "John",
                });

            context.SaveChanges();
        }
    }
}