using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Services.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(string username);
    string GenerateRefreshToken();
    Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken data);
    Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken data);
    Task<RefreshToken> FindRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteExpiredRefreshTokenExceptThisAsync(long userId, string refreshToken);
}
