using Application.Abstractions;
using Domain.Entities.Routes;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RouteStops;

public sealed class RouteStopRepository : IRouteStopRepository
{
    private readonly AppDbContext _context;

    public RouteStopRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RouteStop?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<RouteStop>()
            .Include(rs => rs.Route)
                .ThenInclude(r => r.OriginAirport)
            .Include(rs => rs.Route)
                .ThenInclude(r => r.DestinationAirport)
            .Include(rs => rs.StopAirport)
            .AsTracking()
            .FirstOrDefaultAsync(rs => rs.Id == id, ct);

    public Task<IReadOnlyList<RouteStop>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<RouteStop>()
            .Include(rs => rs.Route)
                .ThenInclude(r => r.OriginAirport)
            .Include(rs => rs.Route)
                .ThenInclude(r => r.DestinationAirport)
            .Include(rs => rs.StopAirport)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<RouteStop>)t.Result, ct);

    public async Task<IReadOnlyList<RouteStop>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<RouteStop> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RouteStops
                .Include(rs => rs.Route)
                    .ThenInclude(r => r.OriginAirport)
                .Include(rs => rs.Route)
                    .ThenInclude(r => r.DestinationAirport)
                .Include(rs => rs.StopAirport)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RouteStops
                .Include(rs => rs.Route)
                    .ThenInclude(r => r.OriginAirport)
                .Include(rs => rs.Route)
                    .ThenInclude(r => r.DestinationAirport)
                .Include(rs => rs.StopAirport)
                .Where(rs =>
                    EF.Functions.ILike(rs.StopAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.StopAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.DestinationAirport.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rs => rs.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<RouteStop> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RouteStops.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RouteStops
                .Where(rs =>
                    EF.Functions.ILike(rs.StopAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.StopAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(rs.Route.DestinationAirport.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(RouteStop routeStop, CancellationToken ct = default)
    {
        _context.RouteStops.Add(routeStop);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(RouteStop routeStop, CancellationToken ct = default)
    {
        _context.RouteStops.Update(routeStop);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(RouteStop routeStop, CancellationToken ct = default)
    {
        _context.RouteStops.Remove(routeStop);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int routeId, int order, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.RouteStops
            .AsNoTracking()
            .Where(rs => rs.RouteId == routeId && rs.Order.Value == order);

        if (excludedId.HasValue)
        {
            query = query.Where(rs => rs.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
