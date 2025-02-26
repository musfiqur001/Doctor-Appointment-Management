using System.ComponentModel.DataAnnotations.Schema;

namespace Doctor_Appointment_Management.Utility.Models;
[Table("RefreshToken")]
public class RefreshToken
{
    public long Id { get; set; }
    public string Token { get; set; }
    public long UserId { get; set; }
    public DateTime ExpiresOn { get; set; }
}
