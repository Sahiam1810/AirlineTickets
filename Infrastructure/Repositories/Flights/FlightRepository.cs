using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.Flights;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Flights;

public sealed class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Flight?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Flight>())
            .AsTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public Task<Flight?> GetByCodeAsync(FlightCode flightCode, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Flight>())
            .AsNoTracking()
            .FirstOrDefaultAsync(f => EF.Functions.ILike(f.FlightCode.Value, flightCode.Value), ct);

    public Task<IReadOnlyList<Flight>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Flight>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Flight>)t.Result, ct);

    public async Task<IReadOnlyList<Flight>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Flight> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.Flights)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.Flights)
                .Where(f =>
                    EF.Functions.ILike(f.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(f.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Airline.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Aircraft.Registration.Value, pattern) ||
                    EF.Functions.ILike(f.FlightState.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Flight> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Flights.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Flights
                .Where(f =>
                    EF.Functions.ILike(f.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(f.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Airline.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Aircraft.Registration.Value, pattern) ||
                    EF.Functions.ILike(f.FlightState.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Flight flight, CancellationToken ct = default)
    {
        _context.Flights.Add(flight);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Flight flight, CancellationToken ct = default)
    {
        _context.Flights.Update(flight);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Flight flight, CancellationToken ct = default)
    {
        _context.Flights.Remove(flight);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByCodeAsync(FlightCode flightCode, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.Flights
            .AsNoTracking()
            .Where(f => EF.Functions.ILike(f.FlightCode.Value, flightCode.Value));

        if (excludedId.HasValue)
        {
            query = query.Where(f => f.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<Flight> IncludeDetails(IQueryable<Flight> query) =>
        query
            .Include(f => f.Airline)
            .Include(f => f.Route)
                .ThenInclude(r => r.OriginAirport)
            .Include(f => f.Route)
                .ThenInclude(r => r.DestinationAirport)
            .Include(f => f.Aircraft)
                .ThenInclude(a => a.AircraftModel)
            .Include(f => f.FlightState);
}
