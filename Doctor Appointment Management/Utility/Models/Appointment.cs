using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Appointment_Management.Utility.Models;

[Table("Appointment")]
public class Appointment
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long AppointmentId { get; set; }

    //[Required]
    public string PatientName { get; set; }

    //[Required]
    public string PatientContact { get; set; }

    //[Required]
    public DateTime AppointmentDateTime { get; set; }

    //[ForeignKey("Doctor")]
    public long DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
