using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Owner;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly OwnerService _service;
    private readonly OwnerAccountService _accountService;

    public OwnersController(OwnerService service, OwnerAccountService accountService)
    {
        _service = service;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (User.IsOwnerClient()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue || ownerId.Value != id) return Forbid();
        }
        var owner = await _service.GetByIdAsync(id);
        return owner == null ? NotFound() : Ok(owner);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OwnerCreateDto dto)
    {
        if (User.IsOwnerClient()) return Forbid();
        await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = 0 }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OwnerCreateDto dto)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue || ownerId.Value != id) return Forbid();
        }
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (User.IsOwnerClient()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/account")]
    public async Task<IActionResult> CreateAccount(int id, OwnerAccountCreateDto dto)
    {
        if (!User.IsAdminOrAgencyOwner()) return Forbid();
        await _accountService.CreateAccountAsync(id, dto);
        return NoContent();
    }
}
