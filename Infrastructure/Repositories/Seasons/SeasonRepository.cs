using Application.Abstractions;
using Domain.Entities.Routes;
using Domain.ValueObjects.Seasons;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Seasons;

public sealed class SeasonRepository : ISeasonRepository
{
    private readonly AppDbContext _context;

    public SeasonRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Season?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Season>()
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Season>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Season>)t.Result, ct);

    public async Task<IReadOnlyList<Season>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Season> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Seasons.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Seasons
                .Where(s => EF.Functions.ILike(s.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Season> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Seasons.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Seasons
                .Where(s => EF.Functions.ILike(s.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Season season, CancellationToken ct = default)
    {
        _context.Seasons.Add(season);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Season season, CancellationToken ct = default)
    {
        _context.Seasons.Update(season);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Season season, CancellationToken ct = default)
    {
        _context.Seasons.Remove(season);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(SeasonName name, CancellationToken ct = default) =>
        _context.Seasons
            .AsNoTracking()
            .AnyAsync(s => EF.Functions.ILike(s.Name.Value, name.Value), ct);
}
