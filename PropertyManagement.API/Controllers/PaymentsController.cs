using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Payment;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private const string ScreenPath = "/app/payments";
    private readonly PaymentService _service;

    public PaymentsController(PaymentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (User.IsPartner())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            return Ok(await _service.GetAllForOwnerAsync(ownerId.Value));
        }

        if (!CanView()) return Forbid();
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsPartner())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            var paymentForOwner = await _service.GetByIdForOwnerAsync(ownerId.Value, id);
            return paymentForOwner == null ? NotFound() : Ok(paymentForOwner);
        }

        if (!CanView()) return Forbid();
        var payment = await _service.GetByIdAsync(id);
        return payment == null ? NotFound() : Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PaymentCreateDto dto)
    {
        if (!CanView()) return Forbid();
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PaymentCreateDto dto)
    {
        if (!CanView()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CanView()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    private bool CanView()
    {
        if (User.IsAdmin()) return true;
        return User.IsStaff() && User.HasScreenPermission(ScreenPath);
    }
}
