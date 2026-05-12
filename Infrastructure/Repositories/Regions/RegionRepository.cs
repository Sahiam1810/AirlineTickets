using Application.Abstractions;
using Domain.Entities.Geography;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Regions;

public sealed class RegionRepository : IRegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Region?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Region>()
            .Include(r => r.Country)
            .AsTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<IReadOnlyList<Region>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Region>()
            .Include(r => r.Country)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Region>)t.Result, ct);

    public async Task<IReadOnlyList<Region>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Region> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Regions
                .Include(r => r.Country)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Regions
                .Include(r => r.Country)
                .Where(r =>
                    EF.Functions.ILike(r.Name.Value, pattern) ||
                    EF.Functions.ILike(r.Type.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Region> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Regions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Regions
                .Where(r =>
                    EF.Functions.ILike(r.Name.Value, pattern) ||
                    EF.Functions.ILike(r.Type.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Region region, CancellationToken ct = default)
    {
        _context.Regions.Add(region);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Region region, CancellationToken ct = default)
    {
        _context.Regions.Update(region);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Region region, CancellationToken ct = default)
    {
        _context.Regions.Remove(region);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name, int countryId, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.Regions
            .AsNoTracking()
            .AnyAsync(r =>
                r.CountryId == countryId &&
                EF.Functions.ILike(r.Name.Value, pattern), ct);
    }
}
