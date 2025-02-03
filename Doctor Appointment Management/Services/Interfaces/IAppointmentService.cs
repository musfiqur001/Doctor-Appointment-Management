using Doctor_Appointment_Management.Utility.Common;
using Doctor_Appointment_Management.Utility.Models;

namespace Doctor_Appointment_Management.Services.Interfaces;

public interface IAppointmentService
{
    Task<ResultData> CreateAppointmentAsync(Appointment dto);
    Task<ResultData> GetAllAppointmentsAsync(int pageNumber, int pageSize);
    Task<ResultData> GetAppointmentByIdAsync(long id);
    Task<ResultData> UpdateAppointmentAsync(long id, Appointment dto);
    Task<ResultData> DeleteAppointmentAsync(long id);
}
