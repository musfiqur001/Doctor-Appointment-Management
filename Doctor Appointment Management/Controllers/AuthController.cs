using Doctor_Appointment_Management.Utility.Dtos;
using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;

namespace Doctor_Appointment_Management.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRegister model)
    {
        Log.Fatal($"User {model.Username} trying to login at " + DateTime.Now);
        var tokenData = await _userService.AuthenticateAsync(model.Username, model.Password);
        return tokenData.Success ? Ok(tokenData) : BadRequest(tokenData);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRegister model)
    {
        var result = await _userService.RegisterUserAsync(model.Username, model.Password);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
