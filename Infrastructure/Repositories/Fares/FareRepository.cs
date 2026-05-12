using Application.Abstractions;
using Domain.Entities.Routes;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Fares;

public sealed class FareRepository : IFareRepository
{
    private readonly AppDbContext _context;

    public FareRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Fare?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Fare>())
            .AsTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public Task<IReadOnlyList<Fare>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Fare>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Fare>)t.Result, ct);

    public async Task<IReadOnlyList<Fare>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Fare> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.Fares)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.Fares)
                .Where(f =>
                    EF.Functions.ILike(f.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(f.PassengerType.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Season.Name.Value, pattern))
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
        IQueryable<Fare> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Fares.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Fares
                .Where(f =>
                    EF.Functions.ILike(f.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Route.DestinationAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(f.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(f.PassengerType.Name.Value, pattern) ||
                    EF.Functions.ILike(f.Season.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Fare fare, CancellationToken ct = default)
    {
        _context.Fares.Add(fare);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Fare fare, CancellationToken ct = default)
    {
        _context.Fares.Update(fare);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Fare fare, CancellationToken ct = default)
    {
        _context.Fares.Remove(fare);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        DateOnly? validFrom,
        DateOnly? validTo,
        int? excludedId = null,
        CancellationToken ct = default)
    {
        var query = _context.Fares
            .AsNoTracking()
            .Where(f =>
                f.RouteId == routeId &&
                f.CabinTypeId == cabinTypeId &&
                f.PassengerTypeId == passengerTypeId &&
                f.SeasonId == seasonId &&
                f.ValidFrom == validFrom &&
                f.ValidTo == validTo);

        if (excludedId.HasValue)
        {
            query = query.Where(f => f.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<Fare> IncludeDetails(IQueryable<Fare> query) =>
        query
            .Include(f => f.Route)
                .ThenInclude(r => r.OriginAirport)
            .Include(f => f.Route)
                .ThenInclude(r => r.DestinationAirport)
            .Include(f => f.CabinType)
            .Include(f => f.PassengerType)
            .Include(f => f.Season);
}
