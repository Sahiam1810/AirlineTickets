using Application.Abstractions;
using Domain.Entities.Airlines;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Airports;

public sealed class AirportRepository : IAirportRepository
{
    private readonly AppDbContext _context;

    public AirportRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Airport?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Airport>()
            .Include(a => a.City)
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<IReadOnlyList<Airport>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Airport>()
            .Include(a => a.City)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Airport>)t.Result, ct);

    public async Task<IReadOnlyList<Airport>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Airport> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Airports
                .Include(a => a.City)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Airports
                .Include(a => a.City)
                .Where(a =>
                    EF.Functions.ILike(a.Name.Value, pattern) ||
                    EF.Functions.ILike(a.IataCode.Value, pattern) ||
                    (a.IcaoCode != null && EF.Functions.ILike(a.IcaoCode.Value, pattern)) ||
                    EF.Functions.ILike(a.City.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Airport> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Airports.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Airports
                .Where(a =>
                    EF.Functions.ILike(a.Name.Value, pattern) ||
                    EF.Functions.ILike(a.IataCode.Value, pattern) ||
                    (a.IcaoCode != null && EF.Functions.ILike(a.IcaoCode.Value, pattern)) ||
                    EF.Functions.ILike(a.City.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Airport airport, CancellationToken ct = default)
    {
        _context.Airports.Add(airport);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Airport airport, CancellationToken ct = default)
    {
        _context.Airports.Update(airport);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Airport airport, CancellationToken ct = default)
    {
        _context.Airports.Remove(airport);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByIataCodeAsync(string iataCode, CancellationToken ct = default)
    {
        var pattern = iataCode.Trim().ToUpperInvariant();

        return _context.Airports
            .AsNoTracking()
            .AnyAsync(a => EF.Functions.ILike(a.IataCode.Value, pattern), ct);
    }

    public Task<bool> ExistsByIcaoCodeAsync(string? icaoCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(icaoCode))
        {
            return Task.FromResult(false);
        }

        var pattern = icaoCode.Trim().ToUpperInvariant();

        return _context.Airports
            .AsNoTracking()
            .AnyAsync(a => a.IcaoCode != null && EF.Functions.ILike(a.IcaoCode.Value, pattern), ct);
    }
}
