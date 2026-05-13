using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PaymentMethodTypes;

public sealed class PaymentMethodTypeRepository : IPaymentMethodTypeRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMethodType?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.PaymentMethodTypes
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<PaymentMethodType>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PaymentMethodTypes
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PaymentMethodType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.PaymentMethodTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search.Trim()}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.PaymentMethodTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search.Trim()}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default)
    {
        await _context.PaymentMethodTypes.AddAsync(paymentMethodType, ct);
    }

    public Task UpdateAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default)
    {
        _context.PaymentMethodTypes.Update(paymentMethodType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default)
    {
        _context.PaymentMethodTypes.Remove(paymentMethodType);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var normalizedName = name.Trim();

        return await _context.PaymentMethodTypes
            .AnyAsync(x => EF.Functions.ILike(x.Name, normalizedName), ct);
    }
}
