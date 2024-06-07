namespace APBD10.models.DTOs;

public class PatientPrescriptionDTO
{
    public PatientDTO Patient { get; set; }
    public List<PrescriptionDTO> PrescriptionList { get; set; }
}