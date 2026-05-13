using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CardIssuers;

public sealed class CardIssuerRepository : ICardIssuerRepository
{
    private readonly AppDbContext _context;

    public CardIssuerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CardIssuer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.CardIssuers
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<CardIssuer>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.CardIssuers
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CardIssuer>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.CardIssuers.AsNoTracking();

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
        var query = _context.CardIssuers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search.Trim()}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(CardIssuer cardIssuer, CancellationToken ct = default)
    {
        await _context.CardIssuers.AddAsync(cardIssuer, ct);
    }

    public Task UpdateAsync(CardIssuer cardIssuer, CancellationToken ct = default)
    {
        _context.CardIssuers.Update(cardIssuer);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CardIssuer cardIssuer, CancellationToken ct = default)
    {
        _context.CardIssuers.Remove(cardIssuer);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var normalizedName = name.Trim();

        return await _context.CardIssuers
            .AnyAsync(x => EF.Functions.ILike(x.Name, normalizedName), ct);
    }
}
