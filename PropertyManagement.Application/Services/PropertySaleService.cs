using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Sale;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class PropertySaleService
{
    private readonly IPropertySaleRepository _saleRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IBuyerClientRepository _buyerRepo;

    public PropertySaleService(IPropertySaleRepository saleRepo, IPropertyRepository propertyRepo, IBuyerClientRepository buyerRepo)
    {
        _saleRepo = saleRepo;
        _propertyRepo = propertyRepo;
        _buyerRepo = buyerRepo;
    }

    public async Task<List<SaleResponseDto>> GetAllAsync()
    {
        var sales = await _saleRepo.GetAllAsync();
        return sales.Select(Map).ToList();
    }

    public async Task<List<SaleResponseDto>> GetAllForOwnerAsync(int ownerId)
    {
        var sales = await _saleRepo.GetAllByOwnerIdAsync(ownerId);
        return sales.Select(Map).ToList();
    }

    public async Task<SaleResponseDto?> GetByIdAsync(int id)
    {
        var sale = await _saleRepo.GetByIdAsync(id);
        return sale == null ? null : Map(sale);
    }

    public async Task<SaleResponseDto?> GetByIdForOwnerAsync(int ownerId, int id)
    {
        var sale = await _saleRepo.GetByIdByOwnerIdAsync(ownerId, id);
        return sale == null ? null : Map(sale);
    }

    public async Task CreateAsync(SaleCreateDto dto)
    {
        var property = await _propertyRepo.GetByIdAsync(dto.PropertyId)
            ?? throw new KeyNotFoundException($"Property {dto.PropertyId} not found");
        if (property.Status == PropertyStatus.Sold)
            throw new System.InvalidOperationException("Property is already sold");

        var buyer = await _buyerRepo.GetByIdAsync(dto.BuyerClientId)
            ?? throw new KeyNotFoundException($"Buyer {dto.BuyerClientId} not found");

        var sale = new PropertySale
        {
            PropertyId = dto.PropertyId,
            BuyerClientId = dto.BuyerClientId,
            SalePrice = dto.SalePrice,
            DeedNumber = dto.DeedNumber,
            SoldAt = dto.SoldAt
        };

        await _saleRepo.AddAsync(sale);

        property.Status = PropertyStatus.Sold;
        property.UpdatedAt = System.DateTime.UtcNow;
        await _propertyRepo.UpdateAsync(property);
    }

    private static SaleResponseDto Map(PropertySale sale)
    {
        return new SaleResponseDto
        {
            Id = sale.Id,
            PropertyId = sale.PropertyId,
            BuyerClientId = sale.BuyerClientId,
            SalePrice = sale.SalePrice,
            DeedNumber = sale.DeedNumber,
            SoldAt = sale.SoldAt,
            CreatedAt = sale.CreatedAt
        };
    }
}
