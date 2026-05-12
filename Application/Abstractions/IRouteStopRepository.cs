using Domain.Entities.Routes;

namespace Application.Abstractions;

public interface IRouteStopRepository
{
    Task<RouteStop?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<RouteStop>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RouteStop>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(RouteStop routeStop, CancellationToken ct = default);
    Task UpdateAsync(RouteStop routeStop, CancellationToken ct = default);
    Task RemoveAsync(RouteStop routeStop, CancellationToken ct = default);
    Task<bool> ExistsAsync(int routeId, int order, int? excludedId = null, CancellationToken ct = default);
}
