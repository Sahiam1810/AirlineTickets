using Application.Abstractions;
using Domain.Entities.Airlines;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AirportAirlines;

public sealed class AirportAirlineRepository : IAirportAirlineRepository
{
    private readonly AppDbContext _context;

    public AirportAirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<AirportAirline?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<AirportAirline>()
            .Include(e => e.Airport)
            .Include(e => e.Airline)
            .AsTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<IReadOnlyList<AirportAirline>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<AirportAirline>()
            .Include(e => e.Airport)
            .Include(e => e.Airline)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<AirportAirline>)t.Result, ct);

    public async Task<IReadOnlyList<AirportAirline>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<AirportAirline> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AirportAirlines
                .Include(e => e.Airport)
                .Include(e => e.Airline)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AirportAirlines
                .Include(e => e.Airport)
                .Include(e => e.Airline)
                .Where(e =>
                    (e.Terminal != null && EF.Functions.ILike(e.Terminal.Value, pattern)) ||
                    EF.Functions.ILike(e.Airport.Name.Value, pattern) ||
                    EF.Functions.ILike(e.Airport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(e.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(e.Airline.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<AirportAirline> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AirportAirlines.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AirportAirlines
                .Where(e =>
                    (e.Terminal != null && EF.Functions.ILike(e.Terminal.Value, pattern)) ||
                    EF.Functions.ILike(e.Airport.Name.Value, pattern) ||
                    EF.Functions.ILike(e.Airport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(e.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(e.Airline.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(AirportAirline airportAirline, CancellationToken ct = default)
    {
        _context.AirportAirlines.Add(airportAirline);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(AirportAirline airportAirline, CancellationToken ct = default)
    {
        _context.AirportAirlines.Update(airportAirline);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AirportAirline airportAirline, CancellationToken ct = default)
    {
        _context.AirportAirlines.Remove(airportAirline);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int airportId, int airlineId, CancellationToken ct = default) =>
        _context.AirportAirlines
            .AsNoTracking()
            .AnyAsync(e => e.AirportId == airportId && e.AirlineId == airlineId, ct);
}
