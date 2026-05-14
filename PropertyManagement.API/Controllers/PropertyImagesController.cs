using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.PropertyImage;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/properties/{propertyId:int}/images")]
public class PropertyImagesController : ControllerBase
{
    private readonly PropertyImageService _service;

    public PropertyImagesController(PropertyImageService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int propertyId)
    {
        if (!TryGetAccess(out var ownerId, out var error)) return error!;
        return Ok(await _service.GetByPropertyIdAsync(propertyId, ownerId));
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)]
    public async Task<IActionResult> Upload(int propertyId, [FromForm] IFormFile file, [FromForm] bool isPrimary = false, [FromForm] int? sortOrder = null, CancellationToken cancellationToken = default)
    {
        if (file == null) return BadRequest("File is required");
        if (!TryGetAccess(out var ownerId, out var error)) return error!;
        await using var stream = file.OpenReadStream();
        var image = await _service.UploadAsync(
            propertyId,
            ownerId,
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            isPrimary,
            sortOrder,
            cancellationToken);

        return Ok(image);
    }

    [HttpDelete("{imageId:int}")]
    public async Task<IActionResult> Delete(int propertyId, int imageId, CancellationToken cancellationToken = default)
    {
        if (!TryGetAccess(out var ownerId, out var error)) return error!;
        await _service.DeleteAsync(propertyId, imageId, ownerId, cancellationToken);
        return NoContent();
    }

    [HttpPut("{imageId:int}/primary")]
    public async Task<IActionResult> SetPrimary(int propertyId, int imageId)
    {
        if (!TryGetAccess(out var ownerId, out var error)) return error!;
        await _service.SetPrimaryAsync(propertyId, imageId, ownerId);
        return NoContent();
    }

    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder(int propertyId, PropertyImageReorderDto dto)
    {
        if (!TryGetAccess(out var ownerId, out var error)) return error!;
        await _service.ReorderAsync(propertyId, dto, ownerId);
        return NoContent();
    }

    private bool TryGetAccess(out int? ownerId, out IActionResult? error)
    {
        if (!User.IsPartner() && !User.IsStaff())
        {
            ownerId = null;
            error = Forbid();
            return false;
        }

        ownerId = null;
        error = null;
        return true;
    }
}
