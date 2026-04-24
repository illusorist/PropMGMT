using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Payment;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class PaymentService
{
    private readonly IPaymentRepository _repo;
    public PaymentService(IPaymentRepository repo) => _repo = repo;

    public async Task<List<PaymentResponseDto>> GetAllAsync()
    {
        var payments = await _repo.GetAllAsync();
        return payments.Select(p => new PaymentResponseDto
        {
            Id = p.Id,
            ContractId = p.ContractId,
            DueDate = p.DueDate,
            PaidDate = p.PaidDate,
            Amount = p.Amount,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<PaymentResponseDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return null;
        return new PaymentResponseDto
        {
            Id = p.Id,
            ContractId = p.ContractId,
            DueDate = p.DueDate,
            PaidDate = p.PaidDate,
            Amount = p.Amount,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        };
    }

    public async Task<List<PaymentResponseDto>> GetAllForOwnerAsync(int ownerId)
    {
        var payments = await _repo.GetAllByOwnerIdAsync(ownerId);
        return payments.Select(p => new PaymentResponseDto
        {
            Id = p.Id,
            ContractId = p.ContractId,
            DueDate = p.DueDate,
            PaidDate = p.PaidDate,
            Amount = p.Amount,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<PaymentResponseDto?> GetByIdForOwnerAsync(int ownerId, int id)
    {
        var p = await _repo.GetByIdByOwnerIdAsync(ownerId, id);
        if (p == null) return null;
        return new PaymentResponseDto
        {
            Id = p.Id,
            ContractId = p.ContractId,
            DueDate = p.DueDate,
            PaidDate = p.PaidDate,
            Amount = p.Amount,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        };
    }

    public async Task CreateAsync(PaymentCreateDto dto)
    {
        var payment = new Payment
        {
            ContractId = dto.ContractId,
            DueDate = dto.DueDate,
            PaidDate = dto.PaidDate,
            Amount = dto.Amount,
            Status = dto.Status
        };
        await _repo.AddAsync(payment);
    }

    public async Task UpdateAsync(int id, PaymentCreateDto dto)
    {
        var payment = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Payment {id} not found");
        payment.ContractId = dto.ContractId;
        payment.DueDate = dto.DueDate;
        payment.PaidDate = dto.PaidDate;
        payment.Amount = dto.Amount;
        payment.Status = dto.Status;
        payment.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(payment);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
