using Doctor_Appointment_Management.DataContext;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Utility.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Doctor_Appointment_Management.Repositories.Implementations;

public class JwtRepository : IJwtRepository
{
    private AppointmentDataContext _dbContext;

    public JwtRepository(AppointmentDataContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken data)
    {
        _dbContext.RefreshTokens.Add(data);
        await _dbContext.SaveChangesAsync();
        return data;
    }
    public async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken data)
    {
        _dbContext.RefreshTokens.Update(data);
        await _dbContext.SaveChangesAsync();
        return data;
    }

    public async Task<RefreshToken> FindRefreshTokenAsync(string refreshToken)
    {
        var data = await _dbContext.RefreshTokens
                                   .FirstOrDefaultAsync(r => r.Token == refreshToken && r.ExpiresOn > DateTime.UtcNow);

        return data;
    }

    public async Task<bool> DeleteExpiredRefreshTokenExceptThisAsync(long userId, string refreshToken)
    {
        var expiredTokens = await _dbContext.RefreshTokens.Where(x=>x.UserId == userId && x.Token!= refreshToken && x.ExpiresOn<DateTime.Now).ToListAsync();
        _dbContext.RefreshTokens.RemoveRange(expiredTokens);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

}
