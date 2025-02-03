using Azure;
using Doctor_Appointment_Management.Utility.Models;
using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Doctor_Appointment_Management.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
    {
        if (appointment == null)
            return BadRequest("Appointment data is required");
        var result = await _appointmentService.CreateAppointmentAsync(appointment);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /appointments
    // Get all appointments
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments(int pageNumber=1, int pageSize=10)
    {
        var result = await _appointmentService.GetAllAppointmentsAsync(pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /appointments/{id}
    // Get an appointment by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(long id)
    {
        var result = await _appointmentService.GetAppointmentByIdAsync(id);
        if (result == null)
            return NotFound($"Appointment with ID {id} not found");
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PUT /appointments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(long id, [FromBody] Appointment appointment)
    {
        if (appointment == null)
            return BadRequest("Appointment data is required");
        var result = await _appointmentService.UpdateAppointmentAsync(id, appointment);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /appointments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(long id)
    {
        var result = await _appointmentService.DeleteAppointmentAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
