using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Doctor_Appointment_Management.Utility.Common;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Services.Implementations;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IJwtRepository _jwtRepository;

    public JwtService(IConfiguration configuration, IJwtRepository jwtRepository)
    {
        _configuration = configuration;
        _jwtRepository = jwtRepository;
    }

    public string GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };
        var jwtSettings = new
        {
            key = _configuration.GetSection("JwtSettings:SecretKey").Value,
            Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
            Audience = _configuration.GetSection("JwtSettings:Audience").Value,
            ExpiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes")
        };
        if (jwtSettings is null)
            throw new Exception("jwt settings is null");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken data)
    {
        return await _jwtRepository.CreateRefreshTokenAsync(data);
    }
    public async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken data)
    {
        return await _jwtRepository.UpdateRefreshTokenAsync(data);
    }
    public async Task<RefreshToken> FindRefreshTokenAsync(string refreshToken)
    {
        return await _jwtRepository.FindRefreshTokenAsync(refreshToken);
    }
    public async Task<bool> DeleteExpiredRefreshTokenExceptThisAsync(long userId, string refreshToken)
    {
        return await _jwtRepository.DeleteExpiredRefreshTokenExceptThisAsync(userId, refreshToken);
    }

}
