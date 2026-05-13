using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.InvoiceItems;

public sealed class InvoiceItemRepository : IInvoiceItemRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceItem?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.InvoiceItems
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<InvoiceItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.InvoiceItems
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<InvoiceItem>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.InvoiceItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Description, $"%{search}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.InvoiceItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Description, $"%{search}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(InvoiceItem invoiceItem, CancellationToken ct = default)
    {
        await _context.InvoiceItems.AddAsync(invoiceItem, ct);
    }

    public Task UpdateAsync(InvoiceItem invoiceItem, CancellationToken ct = default)
    {
        _context.InvoiceItems.Update(invoiceItem);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(InvoiceItem invoiceItem, CancellationToken ct = default)
    {
        _context.InvoiceItems.Remove(invoiceItem);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int invoiceId, string description, CancellationToken ct = default)
    {
        return await _context.InvoiceItems
            .AnyAsync(x => x.InvoiceId == invoiceId && x.Description == description, ct);
    }

    public async Task<bool> ExistsByInvoiceAsync(int invoiceId, CancellationToken ct = default)
    {
        return await _context.InvoiceItems
            .AnyAsync(x => x.InvoiceId == invoiceId, ct);
    }
}
