using Doctor_Appointment_Management.Repositories.Implementations.Common;
using Doctor_Appointment_Management.Repositories.Implementations;
using Doctor_Appointment_Management.Repositories.Interfaces.Common;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Services.Implementations;
using Doctor_Appointment_Management.Services.Interfaces;
using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Utility;

public static class DependencyInjection
{
    public static void ConfigureDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
    }
}
