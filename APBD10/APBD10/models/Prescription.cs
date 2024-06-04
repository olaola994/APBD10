using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD10.models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    [ForeignKey(nameof(IdPatient))]
    public Patient Patient { get; set; }
    
    public int IdDoctor { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public Doctor Doctor { get; set; }
    
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; }
}