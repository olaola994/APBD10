using System.ComponentModel.DataAnnotations;

namespace APBD10.models;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; }
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    public ICollection<Prescription> Prescription { get; set; }
}