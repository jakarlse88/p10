using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Abarnathy.DemographicsService.Models;

namespace Abarnathy.DemographicsService.Data
{
    public partial class DemographicsDbContext : DbContext
    {
        public DemographicsDbContext()
        {
        }

        public DemographicsDbContext(DbContextOptions<DemographicsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patient { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<PhoneNumber> PhoneNumber { get; set; }
        public DbSet<PatientAddress> PatientAddress { get; set; }
        public DbSet<PatientPhoneNumber> PatientPhoneNumbers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Town)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasColumnName("ZIPCode")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasIndex(e => e.SexId);

                entity.Property(e => e.FamilyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GivenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Sex")
                    .IsRequired();
            });

            modelBuilder.Entity<PatientAddress>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.AddressId });

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PatientAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientAddress_Address");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAddresses)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientAddress_Patient");
            });

            modelBuilder.Entity<PatientPhoneNumber>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.PhoneNumberId });

                entity.HasOne(a => a.PhoneNumber)
                    .WithMany(b => b.PatientPhoneNumbers)
                    .HasForeignKey(a => a.PhoneNumberId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientPhoneNumber_PhoneNumber");

                entity.HasOne(a => a.Patient)
                    .WithMany(b => b.PatientPhoneNumbers)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientPhoneNumber_Patient");
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Sex>()
                .HasData(
                    new
                    {
                        Id = 1,
                        Type = "Male"
                    },
                    new
                    {
                        Id = 2,
                        Type = "Female"
                    });

            modelBuilder.Entity<Patient>()
                .HasData(
                    new Patient
                    {
                        Id = 1,
                        SexId = 1,
                        FamilyName = "Smith",
                        GivenName = "James",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 2,
                        SexId = 2,
                        FamilyName = "Lee",
                        GivenName = "Jiyeon",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 3,
                        SexId = 1,
                        FamilyName = "Abe",
                        GivenName = "Masaaki",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 4,
                        SexId = 2,
                        FamilyName = "Svensson",
                        GivenName = "Anna",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 5,
                        SexId = 1,
                        FamilyName = "Haitam",
                        GivenName = "Nurma",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 6,
                        SexId = 2,
                        FamilyName = "Johnson",
                        GivenName = "Lucy",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 7,
                        SexId = 1,
                        FamilyName = "Aleesami",
                        GivenName = "Brian",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 8,
                        SexId = 2,
                        FamilyName = "van Lingen",
                        GivenName = "Elizabeth",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 9,
                        SexId = 2,
                        FamilyName = "Okparaebo",
                        GivenName = "Vivienne",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 10,
                        SexId = 1,
                        FamilyName = "King",
                        GivenName = "Andrew",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 11,
                        SexId = 1,
                        FamilyName = "Locke",
                        GivenName = "Brian",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    },
                    new Patient
                    {
                        Id = 12,
                        SexId = 2,
                        FamilyName = "Wang",
                        GivenName = "Su Lin",
                        DateOfBirth = new DateTime(2010, 1, 1),
                        PatientAddresses = new List<PatientAddress>(),
                        PatientPhoneNumbers = new List<PatientPhoneNumber>()
                    }
                );
        }
    }
}