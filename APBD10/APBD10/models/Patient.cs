using System.ComponentModel.DataAnnotations;

namespace APBD10.models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; }
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    [MaxLength(100)]
    public DateTime BirthDate { get; set; }
    
    public ICollection<Prescription> Prescription { get; set; }
}