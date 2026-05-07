using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Buyer;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BuyersController : ControllerBase
{
    private readonly BuyerClientService _service;

    public BuyersController(BuyerClientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsStaff()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.IsStaff()) return Forbid();
        var buyer = await _service.GetByIdAsync(id);
        return buyer == null ? NotFound() : Ok(buyer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BuyerCreateDto dto)
    {
        if (!User.IsStaff()) return Forbid();
        await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = 0 }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BuyerCreateDto dto)
    {
        if (!User.IsStaff()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsStaff()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
