using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PaymentStates;

public sealed class PaymentStateRepository : IPaymentStateRepository
{
    private readonly AppDbContext _context;

    public PaymentStateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentState?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.PaymentStates
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<PaymentState>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PaymentStates
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PaymentState>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.PaymentStates.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.PaymentStates.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(PaymentState paymentState, CancellationToken ct = default)
    {
        await _context.PaymentStates.AddAsync(paymentState, ct);
    }

    public Task UpdateAsync(PaymentState paymentState, CancellationToken ct = default)
    {
        _context.PaymentStates.Update(paymentState);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PaymentState paymentState, CancellationToken ct = default)
    {
        _context.PaymentStates.Remove(paymentState);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        return await _context.PaymentStates
            .AnyAsync(x => x.Name == name, ct);
    }
}
