using System;
using System.Collections.Generic;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public static class Utilities
    {
        public static void InitializeDbForTests(DemographicsDbContext context)
        {
            context.Database.EnsureCreated();
            var patients = GetSeedingPatients();
            
            context.Address.Add(new Address
            {
                Id = 1,
                StreetName = "Baker St",
                HouseNumber = "6",
                Town = "Baskerville",
                State = "Washington",
                ZipCode = "12345"
            });
            
            context.PhoneNumber.Add(new PhoneNumber
            {
                Id = 1,
                Number = "1234567890"
            });
            
            context.Patient.AddRange(patients);
            
            context.SaveChanges();
        }

        public static void ReinitializeDbForTests(DemographicsDbContext db)
        {
            WipeDb(db);
            InitializeDbForTests(db);
        }

        public static void WipeDb(DemographicsDbContext db)
        {
            db.Database.EnsureDeleted();
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
                    DateOfBirth = new DateTime(1988, 07, 04),
                    SexId = 2,
                },
                new Patient
                {
                    Id = 2,
                    FamilyName = "Smith",
                    GivenName = "John",
                    DateOfBirth = new DateTime(1988, 04, 07),
                    SexId = 1
                }
            };

            return list;
        }
    }
}