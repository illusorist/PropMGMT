using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Sale;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly PropertySaleService _service;

    public SalesController(PropertySaleService service)
    {
        _service = service;
    }

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
        var sale = await _service.GetByIdAsync(id);
        return sale == null ? NotFound() : Ok(sale);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaleCreateDto dto)
    {
        if (!User.IsPartner() && !User.IsStaff()) return Forbid();
        await _service.CreateAsync(dto);
        return NoContent();
    }
}
