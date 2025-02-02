using Doctor_Appointment_Management.DAM.Dtos;
using Doctor_Appointment_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        var token = await _userService.AuthenticateAsync(model.Username, model.Password);
        if (token == null)
            return Unauthorized("Invalid credentials");
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRegister model)
    {
        var user = await _userService.RegisterUserAsync(model.Username, model.Password);
        if (user == null)
            return BadRequest("Username is already taken");
        return Ok(new { Message = "User registered successfully" });
    }
}
