using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.Configuration;
using PropertyManagement.Application.DTOs.Integrations;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private const string RequestsScreenPath = "/app/requests";

    private readonly RequestIngestionService _service;
    private readonly RequestsIngestionOptions _options;

    public RequestsController(
        RequestIngestionService service,
        IOptions<RequestsIngestionOptions> options)
    {
        _service = service;
        _options = options.Value;
    }

    [AllowAnonymous]
    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest(RequestIngestDto dto)
    {
        if (!_options.Enabled)
            return NotFound();

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return StatusCode(500, new { error = "Requests ingestion API key is not configured." });

        var headerName = string.IsNullOrWhiteSpace(_options.HeaderName) ? "X-API-Key" : _options.HeaderName;
        if (!Request.Headers.TryGetValue(headerName, out var incomingApiKey))
            return Unauthorized(new { error = $"Missing {headerName} header." });

        var supplied = incomingApiKey.FirstOrDefault() ?? string.Empty;
        if (!string.Equals(supplied, _options.ApiKey, StringComparison.Ordinal))
            return Unauthorized(new { error = "Invalid API key." });

        try
        {
            var result = await _service.IngestAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("submit")]
    public async Task<IActionResult> Submit(RequestIngestDto dto)
    {
        try
        {
            var result = await _service.IngestAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(RequestIngestDto dto)
    {
        if (!CanViewRequests())
            return Forbid();

        try
        {
            var result = await _service.IngestAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] RequestSearchQueryDto query)
    {
        if (!CanViewRequests()) return Forbid();
        var result = await _service.SearchAsync(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!CanViewRequests()) return Forbid();
        var request = await _service.GetByIdAsync(id);
        return request == null ? NotFound() : Ok(request);
    }

    private bool CanViewRequests()
    {
        if (User.IsAdmin()) return true;
        return User.IsStaff() && User.HasScreenPermission(RequestsScreenPath);
    }
}
