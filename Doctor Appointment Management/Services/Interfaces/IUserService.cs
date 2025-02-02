using Doctor_Appointment_Management.DAM.Models;

namespace Doctor_Appointment_Management.Services.Interfaces;

public interface IUserService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<User?> RegisterUserAsync(string username, string password);
}
