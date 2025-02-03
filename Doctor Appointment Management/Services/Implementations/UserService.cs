using Doctor_Appointment_Management.Utility.Models;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Services.Interfaces;
using System.Security.Cryptography;
using Doctor_Appointment_Management.Utility.Common;

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

    public async Task<ResultData> AuthenticateAsync(string username, string password)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.Password)) // Secure password check
            {
                serviceResponse.Message = "Invalid credentials";
                serviceResponse.Data = null;
                return serviceResponse;
            }
            var result = _jwtService.GenerateToken(user.Username);
            serviceResponse.Data = result;
            serviceResponse.Message = ResponseMessage.Get;
            serviceResponse.Success = true;
            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.InnerException?.Message ?? ResponseMessage.Fail;
            return serviceResponse;
        }
    }
    public async Task<ResultData> RegisterUserAsync(string username, string password)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null) // Username already exists
            {
                serviceResponse.Message = "Username is already taken";
                return serviceResponse;
            }
            var hashedPassword = HashPassword(password);
            var newUser = new User
            {
                Username = username,
                Password = hashedPassword
            };
            var result = await _userRepository.CreateUserAsync(newUser);
            
            newUser.Password = "";
            serviceResponse.Data = newUser.Username;
            serviceResponse.Message = "User registered successfully";
            serviceResponse.Success = true;
            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.InnerException?.Message ?? ResponseMessage.Fail;
            return serviceResponse;
        }
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
