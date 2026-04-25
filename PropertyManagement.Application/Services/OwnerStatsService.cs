using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Owner;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class OwnerStatsService
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IContractRepository _contractRepo;

    public OwnerStatsService(IPropertyRepository propertyRepo, IContractRepository contractRepo)
    {
        _propertyRepo = propertyRepo;
        _contractRepo = contractRepo;
    }

    public async Task<OwnerStatsDto> GetStatsAsync(int ownerId)
    {
        return new OwnerStatsDto
        {
            OwnerId = ownerId,
            TotalProperties = await _propertyRepo.CountByOwnerAsync(ownerId),
            PendingProperties = await _propertyRepo.CountByOwnerAndStatusAsync(ownerId, PropertyStatus.Pending),
            ApprovedProperties = await _propertyRepo.CountByOwnerAndStatusAsync(ownerId, PropertyStatus.Approved),
            RejectedProperties = await _propertyRepo.CountByOwnerAndStatusAsync(ownerId, PropertyStatus.Rejected),
            SoldProperties = await _propertyRepo.CountByOwnerAndStatusAsync(ownerId, PropertyStatus.Sold),
            TotalContracts = await _contractRepo.CountByOwnerAsync(ownerId),
            ActiveContracts = await _contractRepo.CountByOwnerAndStatusAsync(ownerId, ContractStatus.Active),
            PendingContracts = await _contractRepo.CountByOwnerAndStatusAsync(ownerId, ContractStatus.Pending),
            ExpiredContracts = await _contractRepo.CountByOwnerAndStatusAsync(ownerId, ContractStatus.Expired),
            TerminatedContracts = await _contractRepo.CountByOwnerAndStatusAsync(ownerId, ContractStatus.Terminated)
        };
    }
}
