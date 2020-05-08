using System.Collections.Generic;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public static class Utilities
    {
        public static void InitializeDbForTests(DemographicsDbContext db)
        {
            db.Patient.AddRange(GetSeedingPatients());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DemographicsDbContext db)
        {
            db.Patient.RemoveRange(db.Patient);
            InitializeDbForTests(db);
        }
        
        public static IEnumerable<Patient> GetSeedingPatients()
        {
            var list = new List<Patient>
            {
                new Patient
                {
                    FamilyName = "Doe",
                    GivenName = "Jane",
                },
                new Patient
                {
                    FamilyName = "Smith",
                    GivenName = "John",
                }
            };

            return list;
        }
    }
}