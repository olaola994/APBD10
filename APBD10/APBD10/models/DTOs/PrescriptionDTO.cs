namespace APBD10.models.DTOs;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public PatientDTO Patient { get; set; }
    public List<MedicamentDTO> MedicamentList { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDTO Doctor { get; set; }
}