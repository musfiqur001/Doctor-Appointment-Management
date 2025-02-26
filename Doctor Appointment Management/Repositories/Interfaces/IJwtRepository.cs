using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Repositories.Interfaces;

public interface IJwtRepository
{
    Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken data);
    Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken data);
    Task<RefreshToken> FindRefreshTokenAsync(string refreshToken);
}
