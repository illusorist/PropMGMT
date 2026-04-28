using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Contract;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class ContractService
{
    private readonly IContractRepository _repo;
    private readonly IPaymentRepository _paymentRepo;
    public ContractService(IContractRepository repo, IPaymentRepository paymentRepo)
    {
        _repo = repo;
        _paymentRepo = paymentRepo;
    }

    private static void ValidateContractInput(ContractCreateDto dto)
    {
        if (dto.PropertyId <= 0) throw new ArgumentException("Property is required.");
        if (dto.TenantId <= 0) throw new ArgumentException("Tenant is required.");
        if (string.IsNullOrWhiteSpace(dto.DeedNumber)) throw new ArgumentException("Deed number is required.");
        if (dto.MonthlyRent <= 0) throw new ArgumentException("Monthly rent must be greater than zero.");
        if (dto.StartDate > dto.EndDate) throw new ArgumentException("Start date must be on or before end date.");
    }

    public async Task<List<ContractResponseDto>> GetAllAsync()
    {
        var contracts = await _repo.GetAllAsync();
        return contracts.Select(c => new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            DeedNumber = c.DeedNumber,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<ContractResponseDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return null;
        return new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            DeedNumber = c.DeedNumber,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<List<ContractResponseDto>> GetAllForOwnerAsync(int ownerId)
    {
        var contracts = await _repo.GetAllByOwnerIdAsync(ownerId);
        return contracts.Select(c => new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            DeedNumber = c.DeedNumber,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<ContractResponseDto?> GetByIdForOwnerAsync(int ownerId, int id)
    {
        var c = await _repo.GetByIdByOwnerIdAsync(ownerId, id);
        if (c == null) return null;
        return new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            DeedNumber = c.DeedNumber,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<ContractResponseDto> CreateAsync(ContractCreateDto dto)
    {
        ValidateContractInput(dto);

        var contract = new Contract
        {
            PropertyId = dto.PropertyId,
            TenantId = dto.TenantId,
            DeedNumber = dto.DeedNumber,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            MonthlyRent = dto.MonthlyRent,
            Status = dto.Status
        };
        await _repo.AddAsync(contract);

        // If contract is created already Active, generate payments immediately
        if (contract.Status == ContractStatus.Active)
        {
            await GeneratePaymentsAsync(contract);
        }

        return new ContractResponseDto
        {
            Id = contract.Id,
            PropertyId = contract.PropertyId,
            TenantId = contract.TenantId,
            DeedNumber = contract.DeedNumber,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            MonthlyRent = contract.MonthlyRent,
            Status = contract.Status,
            CreatedAt = contract.CreatedAt
        };
    }

    public async Task UpdateAsync(int id, ContractCreateDto dto)
    {
        ValidateContractInput(dto);

        var contract = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Contract {id} not found");
        
        var wasActive = contract.Status == ContractStatus.Active;
        var isBecomingActive = dto.Status == ContractStatus.Active && !wasActive;

        contract.PropertyId = dto.PropertyId;
        contract.TenantId = dto.TenantId;
        contract.DeedNumber = dto.DeedNumber;
        contract.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
        contract.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);
        contract.MonthlyRent = dto.MonthlyRent;
        contract.Status = dto.Status;
        contract.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(contract);

        // Generate payments if contract is becoming Active and no payments exist yet
        if (isBecomingActive)
        {
            await GeneratePaymentsAsync(contract);
        }
    }

    private async Task GeneratePaymentsAsync(Contract contract)
    {
        // Check if payments already exist for this contract
        var existingPayments = await _paymentRepo.GetAllByContractIdAsync(contract.Id);
        if (existingPayments.Any())
        {
            return; // Payments already generated
        }

        var payments = new List<Payment>();
        var currentDate = new DateTime(contract.StartDate.Year, contract.StartDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        // Generate a payment for the first day of each month from start date to end date
        while (currentDate <= contract.EndDate)
        {
            var dueDate = currentDate;
            
            // Only add payment if the due date is within the contract period
            if (dueDate >= contract.StartDate && dueDate <= contract.EndDate)
            {
                payments.Add(new Payment
                {
                    ContractId = contract.Id,
                    DueDate = dueDate,
                    Amount = contract.MonthlyRent,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Move to the first day of next month (preserve UTC kind)
            currentDate = DateTime.SpecifyKind(currentDate.AddMonths(1), DateTimeKind.Utc);
        }

        // Add all generated payments to the repository
        foreach (var payment in payments)
        {
            await _paymentRepo.AddAsync(payment);
        }
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
