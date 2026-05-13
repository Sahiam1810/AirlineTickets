using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.InvoiceItemTypes;

public sealed class InvoiceItemTypeRepository : IInvoiceItemTypeRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceItemType?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.InvoiceItemTypes
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<InvoiceItemType>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.InvoiceItemTypes
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<InvoiceItemType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.InvoiceItemTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name.Value, $"%{search}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.InvoiceItemTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name.Value, $"%{search}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default)
    {
        await _context.InvoiceItemTypes.AddAsync(invoiceItemType, ct);
    }

    public Task UpdateAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default)
    {
        _context.InvoiceItemTypes.Update(invoiceItemType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default)
    {
        _context.InvoiceItemTypes.Remove(invoiceItemType);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        return await _context.InvoiceItemTypes
            .AnyAsync(x => x.Name.Value == name, ct);
    }
}
