using Doctor_Appointment_Management.Utility.Models;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Services.Interfaces;
using System.Security.Cryptography;
using Doctor_Appointment_Management.Utility.Common;
using Doctor_Appointment_Management.Repositories.Implementations;

namespace Doctor_Appointment_Management.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService, IJwtRepository jwtRepository)
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
            var accessToken = _jwtService.GenerateToken(user.Username);
            var refreshToken = new RefreshToken()
            {
                Id = 0,
                UserId = user.UserId,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };
            await _jwtService.CreateRefreshTokenAsync(refreshToken);
            serviceResponse.Data = new { accessToken, refreshToken = refreshToken.Token };
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

    public async Task<ResultData> LogInUserWithRefreshToken(string refreshToken = "Nothing Send From User")
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var retrivedRefreshToken = await _jwtService.FindRefreshTokenAsync(refreshToken);
            if (retrivedRefreshToken == null)
                throw new ApplicationException("The refresh token has expired");
            var retrivedRefreshTokenUser = await _userRepository.GetUserByIdAsync(retrivedRefreshToken.UserId);
            if (retrivedRefreshTokenUser == null)
                throw new ApplicationException("The refresh token has expired");
            await _jwtService.DeleteExpiredRefreshTokenExceptThisAsync(retrivedRefreshToken.UserId, retrivedRefreshToken.Token);
            var accessToken = _jwtService.GenerateToken(retrivedRefreshTokenUser.Username);
            retrivedRefreshToken.Token = _jwtService.GenerateRefreshToken();
            retrivedRefreshToken.ExpiresOn = DateTime.UtcNow.AddDays(7);
            await _jwtService.UpdateRefreshTokenAsync(retrivedRefreshToken);

            serviceResponse.Data = new { accessToken, refreshToken = retrivedRefreshToken.Token };
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
