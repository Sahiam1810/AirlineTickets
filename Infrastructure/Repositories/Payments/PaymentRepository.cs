using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Payments;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Payments
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Payment>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.Payments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search) && int.TryParse(search.Trim(), out var reservationId))
        {
            query = query.Where(x => x.ReservationId == reservationId);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.Payments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search) && int.TryParse(search.Trim(), out var reservationId))
        {
            query = query.Where(x => x.ReservationId == reservationId);
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(Payment payment, CancellationToken ct = default)
    {
        await _context.Payments.AddAsync(payment, ct);
    }

    public Task UpdateAsync(Payment payment, CancellationToken ct = default)
    {
        _context.Payments.Update(payment);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Payment payment, CancellationToken ct = default)
    {
        _context.Payments.Remove(payment);
        return Task.CompletedTask;
    }
}
