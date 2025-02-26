using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Doctor_Appointment_Management.Services.Implementations;

public class JwtService: IJwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
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
}
