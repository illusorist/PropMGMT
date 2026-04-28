using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.User;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserAccountService _service;
    public UsersController(UserAccountService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        var user = await _service.CreateStaffAccountAsync(dto);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsAdmin()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsAdmin()) return Forbid();
        var user = await _service.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(int id, PasswordResetDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.ResetPasswordAsync(id, dto.Password);
        return NoContent();
    }
}
