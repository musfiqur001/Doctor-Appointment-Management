using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Appointment_Management.DAM.Models;

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
    public DateTime AppointmentDate { get; set; }

    //[ForeignKey("Doctor")]
    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; }
}
