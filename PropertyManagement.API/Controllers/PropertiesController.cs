using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Property;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly PropertyService _service;
    public PropertiesController(PropertyService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        var property = await _service.GetByIdAsync(id);
        return property == null ? NotFound() : Ok(property);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PropertyCreateDto dto)
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        var propertyId = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = propertyId }, new { id = propertyId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PropertyCreateDto dto)
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, PropertyStatusUpdateDto dto)
    {
        if (!User.IsStaff()) return Forbid();
        await _service.UpdateStatusAsync(id, dto.Status);
        return NoContent();
    }
}
