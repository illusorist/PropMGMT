using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/public/properties")]
public class PublicPropertiesController : ControllerBase
{
    private readonly PropertyService _service;

    public PublicPropertiesController(PropertyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetPublicAvailableAsync());
    }
}