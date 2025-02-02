using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Appointment_Management.DAM.Models;

public class Doctor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long DoctorId { get; set; }

    [Required]
    public string DoctorName { get; set; }
}
