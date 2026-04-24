using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Application.DTOs.Auth;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized(new { error = "Invalid username or password" });
        return Ok(result);
    }
}
