using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.API.Auth;
using PropertyManagement.Application.DTOs.Contract;
using PropertyManagement.Application.Services;

namespace PropertyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly ContractService _service;
    public ContractsController(ContractService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            return Ok(await _service.GetAllForOwnerAsync(ownerId.Value));
        }
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsOwnerClient())
        {
            var ownerId = User.GetOwnerId();
            if (!ownerId.HasValue) return Forbid();
            var ownerContract = await _service.GetByIdForOwnerAsync(ownerId.Value, id);
            return ownerContract == null ? NotFound() : Ok(ownerContract);
        }

        var contract = await _service.GetByIdAsync(id);
        return contract == null ? NotFound() : Ok(contract);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContractCreateDto dto)
    {
        if (User.IsOwnerClient()) return Forbid();
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ContractCreateDto dto)
    {
        if (User.IsOwnerClient()) return Forbid();
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (User.IsOwnerClient()) return Forbid();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
