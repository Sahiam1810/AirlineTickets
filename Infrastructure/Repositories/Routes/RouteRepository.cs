using Application.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Routes;

public sealed class RouteRepository : IRouteRepository
{
    private readonly AppDbContext _context;

    public RouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Domain.Entities.Routes.Route?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Domain.Entities.Routes.Route>()
            .Include(r => r.OriginAirport)
            .Include(r => r.DestinationAirport)
            .AsTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<IReadOnlyList<Domain.Entities.Routes.Route>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Domain.Entities.Routes.Route>()
            .Include(r => r.OriginAirport)
            .Include(r => r.DestinationAirport)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Domain.Entities.Routes.Route>)t.Result, ct);

    public async Task<IReadOnlyList<Domain.Entities.Routes.Route>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Domain.Entities.Routes.Route> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Routes
                .Include(r => r.OriginAirport)
                .Include(r => r.DestinationAirport)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Routes
                .Include(r => r.OriginAirport)
                .Include(r => r.DestinationAirport)
                .Where(r =>
                    EF.Functions.ILike(r.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(r.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(r.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(r.DestinationAirport.IataCode.Value, pattern))
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
        IQueryable<Domain.Entities.Routes.Route> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Routes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Routes
                .Where(r =>
                    EF.Functions.ILike(r.OriginAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(r.OriginAirport.IataCode.Value, pattern) ||
                    EF.Functions.ILike(r.DestinationAirport.Name.Value, pattern) ||
                    EF.Functions.ILike(r.DestinationAirport.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default)
    {
        _context.Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default)
    {
        _context.Routes.Update(route);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default)
    {
        _context.Routes.Remove(route);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(
        int originAirportId,
        int destinationAirportId,
        int? excludedId = null,
        CancellationToken ct = default)
    {
        var query = _context.Routes
            .AsNoTracking()
            .Where(r => r.OriginAirportId == originAirportId && r.DestinationAirportId == destinationAirportId);

        if (excludedId.HasValue)
        {
            query = query.Where(r => r.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
