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
        if (!User.IsAdminOrAgencyOwner()) return Forbid();
        var user = await _service.CreateStaffAccountAsync(dto);
        return Ok(user);
    }
}
