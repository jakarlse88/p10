using System.Collections.Generic;
using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public static class Utilities
    {
        public static void InitializeDbForTests(DemographicsDbContext db)
        {
            var patients = GetSeedingPatients();
            
            db.Address.Add(new Address
            {
                Id = 1,
                StreetName = "Baker St",
                HouseNumber = "6",
                Town = "Baskerville",
                State = "Washington",
                ZipCode = "12345"
            });

            db.PhoneNumber.Add(new PhoneNumber
            {
                Number = "1234567890"
            });
            
            db.Patient.AddRange(patients);
            
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DemographicsDbContext db)
        {
            WipeDb(db);
            InitializeDbForTests(db);
        }

        public static void WipeDb(DemographicsDbContext db)
        {
            if (db.Patient.Any())
            {
                db.Patient.RemoveRange(db.Patient);
            }
            
            db.SaveChanges();
        }
        
        public static IEnumerable<Patient> GetSeedingPatients()
        {
            var list = new List<Patient>
            {
                new Patient
                {
                    Id = 1,
                    FamilyName = "Doe",
                    GivenName = "Jane",
                    SexId = 2,
                },
                new Patient
                {
                    Id = 2,
                    FamilyName = "Smith",
                    GivenName = "John",
                    SexId = 1
                }
            };

            return list;
        }
    }
}