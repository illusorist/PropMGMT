using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.CommercialListing;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/commercial-listings")]
public class CommercialListingsController : ControllerBase
{
    private const string ScreenPath = "/app/commercial-listings";
    private readonly CommercialListingService _service;
    private readonly IPartnerRepository _partnerRepository;

    public CommercialListingsController(CommercialListingService service, IPartnerRepository partnerRepository)
    {
        _service = service;
        _partnerRepository = partnerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CommercialListingSearchQueryDto query)
    {
        if (User.IsPartner())
        {
            var partnerId = User.GetPartnerId();
            if (!partnerId.HasValue) return Forbid();

            var partner = await _partnerRepository.GetByIdAsync(partnerId.Value);
            if (partner == null || string.IsNullOrWhiteSpace(partner.FullName)) return Forbid();

            var resultForPartner = await _service.SearchAsync(query, partner.FullName);
            return Ok(resultForPartner);
        }

        if (!User.IsOwnerClient() && !CanView()) return Forbid();
        var result = await _service.SearchAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsPartner())
        {
            var partnerId = User.GetPartnerId();
            if (!partnerId.HasValue) return Forbid();

            var partner = await _partnerRepository.GetByIdAsync(partnerId.Value);
            if (partner == null || string.IsNullOrWhiteSpace(partner.FullName)) return Forbid();

            var itemForPartner = await _service.GetByIdAsync(id);
            if (itemForPartner == null) return NotFound();

            if (!string.Equals(itemForPartner.Broker?.Trim(), partner.FullName.Trim(), System.StringComparison.OrdinalIgnoreCase))
                return Forbid();

            return Ok(itemForPartner);
        }

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
