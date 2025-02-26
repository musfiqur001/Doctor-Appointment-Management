using Doctor_Appointment_Management.Utility.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace Doctor_Appointment_Management.DataContext;

public class AppointmentDataContext : DbContext
{
    public AppointmentDataContext(DbContextOptions<AppointmentDataContext> options) : base(options)
    {

    }
    public virtual DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    //public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
}
