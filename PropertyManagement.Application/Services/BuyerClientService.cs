using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Buyer;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class BuyerClientService
{
    private readonly IBuyerClientRepository _repo;

    public BuyerClientService(IBuyerClientRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<BuyerResponseDto>> GetAllAsync()
    {
        var buyers = await _repo.GetAllAsync();
        return buyers.Select(Map).ToList();
    }

    public async Task<BuyerResponseDto?> GetByIdAsync(int id)
    {
        var buyer = await _repo.GetByIdAsync(id);
        return buyer == null ? null : Map(buyer);
    }

    public async Task CreateAsync(BuyerCreateDto dto)
    {
        var buyer = new BuyerClient
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            NationalId = dto.NationalId
        };
        await _repo.AddAsync(buyer);
    }

    public async Task UpdateAsync(int id, BuyerCreateDto dto)
    {
        var buyer = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Buyer {id} not found");
        buyer.FullName = dto.FullName;
        buyer.Phone = dto.Phone;
        buyer.Email = dto.Email;
        buyer.NationalId = dto.NationalId;
        buyer.UpdatedAt = System.DateTime.UtcNow;
        await _repo.UpdateAsync(buyer);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static BuyerResponseDto Map(BuyerClient buyer)
    {
        return new BuyerResponseDto
        {
            Id = buyer.Id,
            FullName = buyer.FullName,
            Phone = buyer.Phone,
            Email = buyer.Email,
            NationalId = buyer.NationalId,
            CreatedAt = buyer.CreatedAt
        };
    }
}
