using Doctor_Appointment_Management.DAM.Models;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Services.Interfaces;
using System.Security.Cryptography;

namespace Doctor_Appointment_Management.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null || !VerifyPassword(password, user.Password)) // Secure password check
            return null;

        return _jwtService.GenerateToken(user.Username);
    }
    public async Task<User?> RegisterUserAsync(string username, string password)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null)
            return null; // Username already exists

        var hashedPassword = HashPassword(password);
        var newUser = new User
        {
            Username = username,
            Password = hashedPassword
        };

        return await _userRepository.CreateUserAsync(newUser);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        var enteredHash = HashPassword(enteredPassword);
        return enteredHash == storedHash;
    }
}
