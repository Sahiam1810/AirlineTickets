using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PaymentMethods;

public sealed class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMethod?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PaymentMethods
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PaymentMethod>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.PaymentMethods.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.CommercialName, $"%{search.Trim()}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.PaymentMethods.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.CommercialName, $"%{search.Trim()}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(PaymentMethod paymentMethod, CancellationToken ct = default)
    {
        await _context.PaymentMethods.AddAsync(paymentMethod, ct);
    }

    public Task UpdateAsync(PaymentMethod paymentMethod, CancellationToken ct = default)
    {
        _context.PaymentMethods.Update(paymentMethod);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PaymentMethod paymentMethod, CancellationToken ct = default)
    {
        _context.PaymentMethods.Remove(paymentMethod);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string commercialName, CancellationToken ct = default)
    {
        var normalizedName = commercialName.Trim();

        return await _context.PaymentMethods
            .AnyAsync(x => x.CommercialName == normalizedName, ct);
    }
}
