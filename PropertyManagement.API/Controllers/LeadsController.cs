using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)]
    public async Task<IActionResult> Submit([FromForm] LeadCreateDto dto, [FromForm] List<IFormFile>? images, CancellationToken cancellationToken)
    {
        var leadId = await _service.CreatePublicAsync(dto);

        if (images != null)
        {
            foreach (var file in images)
            {
                if (file == null || file.Length == 0) continue;
                await using var stream = file.OpenReadStream();
                await _service.AddImageAsync(leadId, stream, file.FileName, file.ContentType, file.Length, cancellationToken);
            }
        }

        var lead = await _service.GetPublicByIdAsync(leadId);
        return CreatedAtAction(nameof(GetById), new { id = leadId }, lead);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] LeadIntent? intent, [FromQuery] LeadStatus? status)
    {
        if (!User.IsAdmin()) return Forbid();
        return Ok(await _service.GetAllAsync(intent, status));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsAdmin()) return Forbid();
        var lead = await _service.GetByIdAsync(id);
        return lead == null ? NotFound() : Ok(lead);
    }

    [Authorize]
    [HttpGet("{id}/images/{imageId:int}/file")]
    public async Task<IActionResult> GetImageFile(int id, int imageId)
    {
        if (!User.IsAdmin()) return Forbid();
        var image = await _service.GetImageFileAsync(id, imageId);
        return File(image.Stream, image.ContentType, image.FileName);
    }

    [Authorize]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        if (!User.IsAdmin()) return Forbid();
        var propertyId = await _service.ApproveAsync(id);
        return Ok(new { propertyId });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LeadUpdateDto dto)
    {
        if (!User.IsAdmin()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}
