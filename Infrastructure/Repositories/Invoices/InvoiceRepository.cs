using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Invoices;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Invoice?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Invoices
            .AsTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IReadOnlyList<Invoice>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Invoices
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Invoice>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Invoice> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Invoices.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Invoices
                .Where(i => EF.Functions.ILike(i.InvoiceNumber, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Invoice> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Invoices.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Invoices
                .Where(i => EF.Functions.ILike(i.InvoiceNumber, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        await _context.Invoices.AddAsync(invoice, ct);
    }

    public Task UpdateAsync(Invoice invoice, CancellationToken ct = default)
    {
        _context.Invoices.Update(invoice);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Invoice invoice, CancellationToken ct = default)
    {
        _context.Invoices.Remove(invoice);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string invoiceNumber, CancellationToken ct = default)
    {
        var normalizedInvoiceNumber = invoiceNumber?.Trim() ?? string.Empty;

        return _context.Invoices
            .AnyAsync(i => i.InvoiceNumber == normalizedInvoiceNumber, ct);
    }

    public Task<bool> ExistsByReservationAsync(int reservationId, CancellationToken ct = default) =>
        _context.Invoices
            .AnyAsync(i => i.ReservationId == reservationId, ct);
}
