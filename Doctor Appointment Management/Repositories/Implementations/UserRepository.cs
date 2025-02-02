using Doctor_Appointment_Management.DAM.Models;
using Doctor_Appointment_Management.DataContext;
using Doctor_Appointment_Management.Repositories.Interfaces;

namespace Doctor_Appointment_Management.Repositories.Implementations;

public class UserRepository: IUserRepository
{
    //private static readonly List<User> Users = new List<User>
    //{
    //    new User { Username = "string", Password = "string" } // Use hashed passwords in production
    //};
    private AppointmentDataContext _dbContext;

    public UserRepository(AppointmentDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await Task.FromResult(_dbContext.Users.FirstOrDefault(u => u.Username == username));
    }
    public async Task<User> CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}
