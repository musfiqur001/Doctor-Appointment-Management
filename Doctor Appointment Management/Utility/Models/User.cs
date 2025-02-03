using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Appointment_Management.Utility.Models;

[Table("User")]
public class User
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
