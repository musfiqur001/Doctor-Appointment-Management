using Doctor_Appointment_Management.Utility.Common;
using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Services.Interfaces;

public interface IUserService
{
    Task<ResultData> AuthenticateAsync(string username, string password);
    Task<ResultData> RegisterUserAsync(string username, string password);
}
