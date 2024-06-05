using APBD10.models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Data;

public class context : DbContext
{
    public context()
    {
    }
    public context(DbContextOptions<context> options) : base(options)
    {
    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        // Seeding Patients
        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1980, 1, 1) },
            new Patient { IdPatient = 2, FirstName = "Jane", LastName = "Doe", BirthDate = new DateTime(1985, 2, 2) }
        );

        // Seeding Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" },
            new Doctor { IdDoctor = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com" }
        );

        // Seeding Medicaments
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicament { IdMedicament = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Tablet" }
        );

        // Seeding Prescriptions
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription { IdPrescription = 1, Date = new DateTime(2023, 6, 1), DueDate = new DateTime(2023, 6, 10), IdPatient = 1, IdDoctor = 1 }
        );

        // Seeding PrescriptionMedicaments
        modelBuilder.Entity<Prescription_Medicament>().HasData(
            new Prescription_Medicament { IdPrescription = 1, IdMedicament = 1, Dose = 100, Details = "Take one tablet daily" },
            new Prescription_Medicament { IdPrescription = 1, IdMedicament = 2, Dose = 200, Details = "Take two tablets daily" }
        );
    }
}