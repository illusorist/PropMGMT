using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.CommercialListing;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class CommercialListingService
{
    private readonly ICommercialListingRepository _repo;

    public CommercialListingService(ICommercialListingRepository repo)
    {
        _repo = repo;
    }

    public async Task<CommercialListingSearchResultDto> SearchAsync(CommercialListingSearchQueryDto query, string? broker = null)
    {
        var (items, total) = await _repo.SearchAsync(
            query.Q,
            query.Status,
            query.Employee,
            broker,
            query.Page,
            query.PageSize,
            query.SortBy,
            query.SortDir);

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : Math.Min(query.PageSize, 100);

        return new CommercialListingSearchResultDto
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = items.Select(MapDto).ToList()
        };
    }

    public async Task<CommercialListingDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null ? null : MapDto(entity);
    }

    public async Task<CommercialListingDto> CreateAsync(CommercialListingUpsertDto dto)
    {
        var entity = new CommercialListing
        {
            RowFlag = TrimValue(dto.RowFlag),
            SerialNumber = TrimValue(dto.SerialNumber),
            ContactDate = TrimValue(dto.ContactDate),
            PropertyStatus = TrimValue(dto.PropertyStatus),
            BrokerageContract = TrimValue(dto.BrokerageContract),
            LicenseNumber = TrimValue(dto.LicenseNumber),
            ContractExpiry = TrimValue(dto.ContractExpiry),
            AdNumber = TrimValue(dto.AdNumber),
            Employee = TrimValue(dto.Employee),
            Broker = TrimValue(dto.Broker),
            OwnerName = TrimValue(dto.OwnerName),
            Mobile1 = TrimValue(dto.Mobile1),
            Mobile2 = TrimValue(dto.Mobile2),
            AvailableUnits = TrimValue(dto.AvailableUnits),
            DeedNumber = TrimValue(dto.DeedNumber),
            PropertyType = TrimValue(dto.PropertyType),
            RoomsCount = TrimValue(dto.RoomsCount),
            BuildingAge = TrimValue(dto.BuildingAge),
            HasElevator = TrimValue(dto.HasElevator),
            OtherDetails = TrimValue(dto.OtherDetails),
            RentAmount = TrimValue(dto.RentAmount),
            PaymentType = TrimValue(dto.PaymentType),
            Location = TrimValue(dto.Location),
            Coordinates = TrimValue(dto.Coordinates),
            HasKey = TrimValue(dto.HasKey),
            PublishedTahmid = TrimValue(dto.PublishedTahmid),
            PublishedBoard = TrimValue(dto.PublishedBoard),
            PublishedDesigns = TrimValue(dto.PublishedDesigns),
            PublishedHaraj = TrimValue(dto.PublishedHaraj),
            PublishedDeal = TrimValue(dto.PublishedDeal),
            PublishedAqar = TrimValue(dto.PublishedAqar),
            PublishedBayut = TrimValue(dto.PublishedBayut),
            PublishedDhaki = TrimValue(dto.PublishedDhaki),
            PublishedWhatsapp = TrimValue(dto.PublishedWhatsapp),
            PublishedTwitter = TrimValue(dto.PublishedTwitter),
            PublishedWhatsappGroup = TrimValue(dto.PublishedWhatsappGroup),
            PublishedWhatsappChannel = TrimValue(dto.PublishedWhatsappChannel),
            PublishedSnapchat = TrimValue(dto.PublishedSnapchat),
            PublishedX = TrimValue(dto.PublishedX),
            PublishedInstagram = TrimValue(dto.PublishedInstagram),
            PublishedTiktok = TrimValue(dto.PublishedTiktok),
            Notes = TrimValue(dto.Notes)
        };

        await _repo.AddAsync(entity);
        return MapDto(entity);
    }

    public async Task<CommercialListingDto> UpdateAsync(int id, CommercialListingUpsertDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Commercial listing {id} not found");

        ApplyIfProvided(dto.RowFlag, v => entity.RowFlag = v);
        ApplyIfProvided(dto.SerialNumber, v => entity.SerialNumber = v);
        ApplyIfProvided(dto.ContactDate, v => entity.ContactDate = v);
        ApplyIfProvided(dto.PropertyStatus, v => entity.PropertyStatus = v);
        ApplyIfProvided(dto.BrokerageContract, v => entity.BrokerageContract = v);
        ApplyIfProvided(dto.LicenseNumber, v => entity.LicenseNumber = v);
        ApplyIfProvided(dto.ContractExpiry, v => entity.ContractExpiry = v);
        ApplyIfProvided(dto.AdNumber, v => entity.AdNumber = v);
        ApplyIfProvided(dto.Employee, v => entity.Employee = v);
        ApplyIfProvided(dto.Broker, v => entity.Broker = v);
        ApplyIfProvided(dto.OwnerName, v => entity.OwnerName = v);
        ApplyIfProvided(dto.Mobile1, v => entity.Mobile1 = v);
        ApplyIfProvided(dto.Mobile2, v => entity.Mobile2 = v);
        ApplyIfProvided(dto.AvailableUnits, v => entity.AvailableUnits = v);
        ApplyIfProvided(dto.DeedNumber, v => entity.DeedNumber = v);
        ApplyIfProvided(dto.PropertyType, v => entity.PropertyType = v);
        ApplyIfProvided(dto.RoomsCount, v => entity.RoomsCount = v);
        ApplyIfProvided(dto.BuildingAge, v => entity.BuildingAge = v);
        ApplyIfProvided(dto.HasElevator, v => entity.HasElevator = v);
        ApplyIfProvided(dto.OtherDetails, v => entity.OtherDetails = v);
        ApplyIfProvided(dto.RentAmount, v => entity.RentAmount = v);
        ApplyIfProvided(dto.PaymentType, v => entity.PaymentType = v);
        ApplyIfProvided(dto.Location, v => entity.Location = v);
        ApplyIfProvided(dto.Coordinates, v => entity.Coordinates = v);
        ApplyIfProvided(dto.HasKey, v => entity.HasKey = v);
        ApplyIfProvided(dto.PublishedTahmid, v => entity.PublishedTahmid = v);
        ApplyIfProvided(dto.PublishedBoard, v => entity.PublishedBoard = v);
        ApplyIfProvided(dto.PublishedDesigns, v => entity.PublishedDesigns = v);
        ApplyIfProvided(dto.PublishedHaraj, v => entity.PublishedHaraj = v);
        ApplyIfProvided(dto.PublishedDeal, v => entity.PublishedDeal = v);
        ApplyIfProvided(dto.PublishedAqar, v => entity.PublishedAqar = v);
        ApplyIfProvided(dto.PublishedBayut, v => entity.PublishedBayut = v);
        ApplyIfProvided(dto.PublishedDhaki, v => entity.PublishedDhaki = v);
        ApplyIfProvided(dto.PublishedWhatsapp, v => entity.PublishedWhatsapp = v);
        ApplyIfProvided(dto.PublishedTwitter, v => entity.PublishedTwitter = v);
        ApplyIfProvided(dto.PublishedWhatsappGroup, v => entity.PublishedWhatsappGroup = v);
        ApplyIfProvided(dto.PublishedWhatsappChannel, v => entity.PublishedWhatsappChannel = v);
        ApplyIfProvided(dto.PublishedSnapchat, v => entity.PublishedSnapchat = v);
        ApplyIfProvided(dto.PublishedX, v => entity.PublishedX = v);
        ApplyIfProvided(dto.PublishedInstagram, v => entity.PublishedInstagram = v);
        ApplyIfProvided(dto.PublishedTiktok, v => entity.PublishedTiktok = v);
        ApplyIfProvided(dto.Notes, v => entity.Notes = v);

        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return MapDto(entity);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static string? TrimValue(string? value) => value?.Trim();

    private static void ApplyIfProvided(string? incoming, Action<string?> apply)
    {
        if (incoming == null) return;
        apply(TrimValue(incoming));
    }

    private static CommercialListingDto MapDto(CommercialListing entity)
    {
        return new CommercialListingDto
        {
            Id = entity.Id,
            RowFlag = entity.RowFlag,
            SerialNumber = entity.SerialNumber,
            ContactDate = entity.ContactDate,
            PropertyStatus = entity.PropertyStatus,
            BrokerageContract = entity.BrokerageContract,
            LicenseNumber = entity.LicenseNumber,
            ContractExpiry = entity.ContractExpiry,
            AdNumber = entity.AdNumber,
            Employee = entity.Employee,
            Broker = entity.Broker,
            OwnerName = entity.OwnerName,
            Mobile1 = entity.Mobile1,
            Mobile2 = entity.Mobile2,
            AvailableUnits = entity.AvailableUnits,
            DeedNumber = entity.DeedNumber,
            PropertyType = entity.PropertyType,
            RoomsCount = entity.RoomsCount,
            BuildingAge = entity.BuildingAge,
            HasElevator = entity.HasElevator,
            OtherDetails = entity.OtherDetails,
            RentAmount = entity.RentAmount,
            PaymentType = entity.PaymentType,
            Location = entity.Location,
            Coordinates = entity.Coordinates,
            HasKey = entity.HasKey,
            PublishedTahmid = entity.PublishedTahmid,
            PublishedBoard = entity.PublishedBoard,
            PublishedDesigns = entity.PublishedDesigns,
            PublishedHaraj = entity.PublishedHaraj,
            PublishedDeal = entity.PublishedDeal,
            PublishedAqar = entity.PublishedAqar,
            PublishedBayut = entity.PublishedBayut,
            PublishedDhaki = entity.PublishedDhaki,
            PublishedWhatsapp = entity.PublishedWhatsapp,
            PublishedTwitter = entity.PublishedTwitter,
            PublishedWhatsappGroup = entity.PublishedWhatsappGroup,
            PublishedWhatsappChannel = entity.PublishedWhatsappChannel,
            PublishedSnapchat = entity.PublishedSnapchat,
            PublishedX = entity.PublishedX,
            PublishedInstagram = entity.PublishedInstagram,
            PublishedTiktok = entity.PublishedTiktok,
            Notes = entity.Notes,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
