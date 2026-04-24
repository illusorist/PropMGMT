using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Application.DTOs.Tenant;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly TenantService _service;
    public TenantsController(TenantService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tenant = await _service.GetByIdAsync(id);
        return tenant == null ? NotFound() : Ok(tenant);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TenantCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = 0 }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TenantCreateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
