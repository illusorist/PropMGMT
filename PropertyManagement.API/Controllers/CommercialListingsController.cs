using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.CommercialListing;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/commercial-listings")]
public class CommercialListingsController : ControllerBase
{
    private const string ScreenPath = "/app/commercial-listings";
    private readonly CommercialListingService _service;

    public CommercialListingsController(CommercialListingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CommercialListingSearchQueryDto query)
    {
        if (!User.IsOwnerClient() && !CanView()) return Forbid();
        var result = await _service.SearchAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsOwnerClient() && !CanView()) return Forbid();
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommercialListingUpsertDto dto)
    {
        if (User.IsOwnerClient()) return Forbid();
        if (!CanView()) return Forbid();
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CommercialListingUpsertDto dto)
    {
        if (User.IsOwnerClient()) return Forbid();
        if (!CanView()) return Forbid();
        var updated = await _service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    private bool CanView()
    {
        if (User.IsAdmin()) return true;
        return User.IsStaff() && User.HasScreenPermission(ScreenPath);
    }
}
