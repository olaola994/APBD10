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
}