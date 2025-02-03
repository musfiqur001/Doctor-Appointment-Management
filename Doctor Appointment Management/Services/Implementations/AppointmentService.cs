using Doctor_Appointment_Management.Utility.Common;
using Doctor_Appointment_Management.Utility.Models;
using Doctor_Appointment_Management.Utility.Pagination;
using Doctor_Appointment_Management.Repositories.Implementations;
using Doctor_Appointment_Management.Repositories.Interfaces;
using Doctor_Appointment_Management.Repositories.Interfaces.Common;
using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Doctor_Appointment_Management.Services.Implementations;

public class AppointmentService : IAppointmentService
{
    //private readonly IAppointmentRepository _appointmentRepository;
    private readonly IGenericRepository<Appointment> _repo;

    public AppointmentService(IGenericRepository<Appointment> repo)
    {
        _repo = repo;
    }

    public async Task<ResultData> CreateAppointmentAsync(Appointment dto)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            Appointment model = new Appointment();
            model.PatientName = dto.PatientName;
            model.PatientContact = dto.PatientContact;
            model.AppointmentDateTime = dto.AppointmentDateTime;
            model.DoctorId = dto.DoctorId;
            await _repo.InsertAsync(model);

            serviceResponse.Data = model;
            serviceResponse.Message = ResponseMessage.Save;
            serviceResponse.Success = true;
            return serviceResponse;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An error occurred while creating the appointment.");

            serviceResponse.Message = ex.InnerException?.Message ?? ResponseMessage.Fail;
            return serviceResponse;
        }
    }

    public async Task<ResultData> DeleteAppointmentAsync(long id)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var result = await GetAppointmentByIdAsync(id);
            if (!result.Success)
            {
                serviceResponse.Message = ResponseMessage.Fail;
                return serviceResponse;
            }
            await _repo.DeleteAsync(id);
            serviceResponse.Data = id.ToString();
            serviceResponse.Message = ResponseMessage.Delete;
            serviceResponse.Success = true;
            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.InnerException?.Message ?? ResponseMessage.Fail;
            return serviceResponse;
        }
    }

    public async Task<ResultData> GetAllAppointmentsAsync(int pageNumber, int pageSize)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var data = _repo.GetAll();
            var totalCount = await data.CountAsync();
            var retrivedData = await data.OrderBy(x => x.AppointmentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new Appointment
                {
                    AppointmentId = x.AppointmentId,
                    PatientName = x.PatientName,
                    PatientContact = x.PatientContact,
                    AppointmentDateTime = x.AppointmentDateTime,
                    DoctorId = x.DoctorId
                })
                .ToListAsync();
            var result = new PaginatedList<object>(retrivedData, totalCount, pageNumber, pageSize);

            if (result == null)
            {
                serviceResponse.Message = ResponseMessage.NotFound;
                return serviceResponse;
            }
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

    public async Task<ResultData> GetAppointmentByIdAsync(long id)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
            {
                serviceResponse.Message = ResponseMessage.NotFound;
                return serviceResponse;
            }
            serviceResponse.Data = data;
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

    public async Task<ResultData> UpdateAppointmentAsync(long id, Appointment dto)
    {
        ResultData serviceResponse = new ResultData();
        try
        {
            if (dto.AppointmentId != id)
            {
                serviceResponse.Message = ResponseMessage.Fail;
                return serviceResponse;
            }
            var existingData = await _repo.GetByIdAsync(dto.AppointmentId);
            if (existingData == null)
            {
                serviceResponse.Message = ResponseMessage.NotFound;
                return serviceResponse;
            }
            existingData.PatientName = dto.PatientName;
            existingData.PatientContact = dto.PatientContact;
            existingData.AppointmentDateTime = dto.AppointmentDateTime;
            existingData.DoctorId = dto.DoctorId;
            await _repo.UpdateAsync(existingData);

            serviceResponse.Data = existingData;
            serviceResponse.Message = ResponseMessage.Update;
            serviceResponse.Success = true;
            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.InnerException?.Message ?? ResponseMessage.Fail;
            return serviceResponse;
        }
    }
}
