using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Lead;
using PropertyManagement.Application.Services;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly LeadService _service;

    public LeadsController(LeadService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("submit")]
    public async Task<IActionResult> Submit(LeadCreateDto dto)
    {
        var leadId = await _service.CreatePublicAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = leadId }, new { id = leadId });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] LeadIntent? intent, [FromQuery] LeadStatus? status)
    {
        if (!User.IsAdminOrAgencyOwner()) return Forbid();
        return Ok(await _service.GetAllAsync(intent, status));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsAdminOrAgencyOwner()) return Forbid();
        var lead = await _service.GetByIdAsync(id);
        return lead == null ? NotFound() : Ok(lead);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LeadUpdateDto dto)
    {
        if (!User.IsAdminOrAgencyOwner()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}