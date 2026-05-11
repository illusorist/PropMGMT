using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Partner;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PartnersController : ControllerBase
{
    private readonly PartnerService _service;

    public PartnersController(PartnerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsAdmin()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (User.IsPartner())
        {
            var partnerId = User.GetPartnerId();
            if (!partnerId.HasValue || partnerId.Value != id) return Forbid();
        }
        else if (!User.IsAdmin())
        {
            return Forbid();
        }

        var partner = await _service.GetByIdAsync(id);
        return partner == null ? NotFound() : Ok(partner);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePartnerDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = Guid.Empty }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePartnerDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/account")]
    public async Task<IActionResult> CreateAccount(Guid id, CreatePartnerAccountDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.CreateAccountAsync(id, dto);
        return NoContent();
    }
}
