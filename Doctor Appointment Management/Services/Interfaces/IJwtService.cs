namespace Doctor_Appointment_Management.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(string username);
}
