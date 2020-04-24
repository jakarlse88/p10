using Microsoft.EntityFrameworkCore;
using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Data
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

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientAddress> PatientAddress { get; set; }
        public virtual DbSet<Sex> Sex { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Zipcode)
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

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Sex");
            });

            modelBuilder.Entity<PatientAddress>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.AddressId });

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientAddress_Address");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientAddress_Patient");
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
