using Application.Abstractions;
using Domain.Entities.Geography;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Countries;

public sealed class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Country?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Country>()
            .Include(c => c.Continent)
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Country>()
            .Include(c => c.Continent)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Country>)t.Result, ct);

    public async Task<IReadOnlyList<Country>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Country> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Countries
                .Include(c => c.Continent)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Countries
                .Include(c => c.Continent)
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
        IQueryable<Country> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Countries.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Countries
                .Where(c => EF.Functions.ILike(c.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Country country, CancellationToken ct = default)
    {
        _context.Countries.Add(country);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Country country, CancellationToken ct = default)
    {
        _context.Countries.Update(country);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Country country, CancellationToken ct = default)
    {
        _context.Countries.Remove(country);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.Countries
            .AsNoTracking()
            .AnyAsync(c => EF.Functions.ILike(c.Name.Value, pattern), ct);
    }
}
