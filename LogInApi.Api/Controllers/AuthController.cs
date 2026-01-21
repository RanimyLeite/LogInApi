using LogInApi.Application.DTOs;
using LogInApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogInApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        await _authService.CreateUserAsync(dto);
        return Ok("User created successfully!");
    }
}