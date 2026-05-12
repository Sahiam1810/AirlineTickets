using Application.Abstractions;
using Domain.Entities.Geography;
using Domain.ValueObjects.Continents;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Continents;

public sealed class ContinentRepository : IContinent
{
    private readonly AppDbContext _context;

    public ContinentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Continent?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Continent>()
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<IReadOnlyList<Continent>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Continent>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Continent>)t.Result, ct);

    public async Task<IReadOnlyList<Continent>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Continent> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Continents.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Continents
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
        IQueryable<Continent> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Continents.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Continents
                .Where(c => EF.Functions.ILike(c.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Continents.Add(continent);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Continents.Update(continent);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Continents.Remove(continent);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(ContinentName name, CancellationToken ct = default) =>
        _context.Continents
            .AnyAsync(c => c.Name == name, ct);

    public Task<Continent?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsNamedAsync(string name, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}