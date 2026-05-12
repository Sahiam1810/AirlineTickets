using Application.Abstractions;
using Domain.Entities.Airlines;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Airlines;

public sealed class AirlineRepository : IAirlineRepository
{
    private readonly AppDbContext _context;

    public AirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Airline?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Airline>()
            .Include(a => a.Country)
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<IReadOnlyList<Airline>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Airline>()
            .Include(a => a.Country)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Airline>)t.Result, ct);

    public async Task<IReadOnlyList<Airline>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Airline> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Airlines
                .Include(a => a.Country)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Airlines
                .Include(a => a.Country)
                .Where(a =>
                    EF.Functions.ILike(a.Name.Value, pattern) ||
                    EF.Functions.ILike(a.IataCode.Value, pattern) ||
                    EF.Functions.ILike(a.Country.Name.Value, pattern))
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
        IQueryable<Airline> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Airlines.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Airlines
                .Where(a =>
                    EF.Functions.ILike(a.Name.Value, pattern) ||
                    EF.Functions.ILike(a.IataCode.Value, pattern) ||
                    EF.Functions.ILike(a.Country.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Airline airline, CancellationToken ct = default)
    {
        _context.Airlines.Add(airline);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Airline airline, CancellationToken ct = default)
    {
        _context.Airlines.Update(airline);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Airline airline, CancellationToken ct = default)
    {
        _context.Airlines.Remove(airline);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByIataCodeAsync(string iataCode, CancellationToken ct = default)
    {
        var pattern = iataCode.Trim().ToUpperInvariant();

        return _context.Airlines
            .AsNoTracking()
            .AnyAsync(a => EF.Functions.ILike(a.IataCode.Value, pattern), ct);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.Airlines
            .AsNoTracking()
            .AnyAsync(a => EF.Functions.ILike(a.Name.Value, pattern), ct);
    }
}
