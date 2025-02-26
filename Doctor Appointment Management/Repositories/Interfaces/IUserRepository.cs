using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByIdAsync(long id);
}
