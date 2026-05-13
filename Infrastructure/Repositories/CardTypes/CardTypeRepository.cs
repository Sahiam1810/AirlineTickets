using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CardTypes;

public sealed class CardTypeRepository : ICardTypeRepository
{
    private readonly AppDbContext _context;

    public CardTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CardType?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.CardTypes
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<CardType>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.CardTypes
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CardType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.CardTypes.AsNoTracking();

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
        var query = _context.CardTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search.Trim()}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(CardType cardType, CancellationToken ct = default)
    {
        await _context.CardTypes.AddAsync(cardType, ct);
    }

    public Task UpdateAsync(CardType cardType, CancellationToken ct = default)
    {
        _context.CardTypes.Update(cardType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CardType cardType, CancellationToken ct = default)
    {
        _context.CardTypes.Remove(cardType);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var normalizedName = name.Trim();

        return await _context.CardTypes
            .AnyAsync(x => EF.Functions.ILike(x.Name, normalizedName), ct);
    }
}
