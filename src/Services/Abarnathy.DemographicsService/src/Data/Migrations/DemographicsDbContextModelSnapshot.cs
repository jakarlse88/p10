﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Abarnathy.DemographicsService.Data.Migrations
{
    [DbContext(typeof(DemographicsDbContext))]
    partial class DemographicsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Town")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnName("ZIPCode")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("SexId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SexId");

                    b.ToTable("Patient");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Smith",
                            GivenName = "James",
                            SexId = 1
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Lee",
                            GivenName = "Jiyeon",
                            SexId = 2
                        },
                        new
                        {
                            Id = 3,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Abe",
                            GivenName = "Masaaki",
                            SexId = 1
                        },
                        new
                        {
                            Id = 4,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Svensson",
                            GivenName = "Anna",
                            SexId = 2
                        },
                        new
                        {
                            Id = 5,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Haitam",
                            GivenName = "Nurma",
                            SexId = 1
                        },
                        new
                        {
                            Id = 6,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Johnson",
                            GivenName = "Lucy",
                            SexId = 2
                        },
                        new
                        {
                            Id = 7,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Aleesami",
                            GivenName = "Brian",
                            SexId = 1
                        },
                        new
                        {
                            Id = 8,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "van Lingen",
                            GivenName = "Elizabeth",
                            SexId = 2
                        },
                        new
                        {
                            Id = 9,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Okparaebo",
                            GivenName = "Vivienne",
                            SexId = 2
                        },
                        new
                        {
                            Id = 10,
                            DateOfBirth = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "King",
                            GivenName = "Andrew",
                            SexId = 1
                        },
                        new
                        {
                            Id = 11,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Locke",
                            GivenName = "Brian",
                            SexId = 1
                        },
                        new
                        {
                            Id = 12,
                            DateOfBirth = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FamilyName = "Wang",
                            GivenName = "Su Lin",
                            SexId = 2
                        });
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.PatientAddress", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("PatientAddress");
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.PatientPhoneNumber", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("PhoneNumberId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "PhoneNumberId");

                    b.HasIndex("PhoneNumberId");

                    b.ToTable("PatientPhoneNumbers");
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("PhoneNumber");
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.Sex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Sex");

                    b.HasData(
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
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.Patient", b =>
                {
                    b.HasOne("Abarnathy.DemographicsService.Models.Sex", "Sex")
                        .WithMany("Patients")
                        .HasForeignKey("SexId")
                        .HasConstraintName("FK_Patient_Sex")
                        .IsRequired();
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.PatientAddress", b =>
                {
                    b.HasOne("Abarnathy.DemographicsService.Models.Address", "Address")
                        .WithMany("PatientAddresses")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_PatientAddress_Address")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Abarnathy.DemographicsService.Models.Patient", "Patient")
                        .WithMany("PatientAddresses")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_PatientAddress_Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Abarnathy.DemographicsService.Models.PatientPhoneNumber", b =>
                {
                    b.HasOne("Abarnathy.DemographicsService.Models.Patient", "Patient")
                        .WithMany("PatientPhoneNumbers")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_PatientPhoneNumber_Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Abarnathy.DemographicsService.Models.PhoneNumber", "PhoneNumber")
                        .WithMany("PatientPhoneNumbers")
                        .HasForeignKey("PhoneNumberId")
                        .HasConstraintName("FK_PatientPhoneNumber_PhoneNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
