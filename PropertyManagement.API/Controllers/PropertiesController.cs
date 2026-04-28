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
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            return Ok(await _service.GetAllForOwnerAsync(ownerId.Value));
        }
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            var ownerProperty = await _service.GetByIdForOwnerAsync(ownerId.Value, id);
            return ownerProperty == null ? NotFound() : Ok(ownerProperty);
        }

        var property = await _service.GetByIdAsync(id);
        return property == null ? NotFound() : Ok(property);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PropertyCreateDto dto)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            await _service.CreateForOwnerAsync(ownerId.Value, dto);
        }
        else
        {
            await _service.CreateAsync(dto);
        }
        return CreatedAtAction(nameof(GetById), new { id = 0 }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PropertyCreateDto dto)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            await _service.UpdateForOwnerAsync(ownerId.Value, id, dto);
        }
        else
        {
            await _service.UpdateAsync(id, dto);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            await _service.DeleteForOwnerAsync(ownerId.Value, id);
        }
        else
        {
            await _service.DeleteAsync(id);
        }
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, PropertyStatusUpdateDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.UpdateStatusAsync(id, dto.Status);
        return NoContent();
    }
}
