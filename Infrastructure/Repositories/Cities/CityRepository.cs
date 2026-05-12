using Application.Abstractions;
using Domain.Entities.Geography;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Cities;

public sealed class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<City?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<City>()
            .Include(c => c.Region)
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<IReadOnlyList<City>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<City>()
            .Include(c => c.Region)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<City>)t.Result, ct);

    public async Task<IReadOnlyList<City>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<City> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Cities
                .Include(c => c.Region)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Cities
                .Include(c => c.Region)
                .Where(c => EF.Functions.ILike(c.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<City> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Cities.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Cities
                .Where(c => EF.Functions.ILike(c.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(City city, CancellationToken ct = default)
    {
        _context.Cities.Add(city);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(City city, CancellationToken ct = default)
    {
        _context.Cities.Update(city);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(City city, CancellationToken ct = default)
    {
        _context.Cities.Remove(city);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name, int regionId, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.Cities
            .AsNoTracking()
            .AnyAsync(c =>
                c.RegionId == regionId &&
                EF.Functions.ILike(c.Name.Value, pattern), ct);
    }
}
