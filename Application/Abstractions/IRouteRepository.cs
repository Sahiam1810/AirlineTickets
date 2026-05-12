namespace Application.Abstractions;

public interface IRouteRepository
{
    Task<Domain.Entities.Routes.Route?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Domain.Entities.Routes.Route>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Domain.Entities.Routes.Route>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default);
    Task UpdateAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default);
    Task RemoveAsync(Domain.Entities.Routes.Route route, CancellationToken ct = default);
    Task<bool> ExistsAsync(int originAirportId, int destinationAirportId, int? excludedId = null, CancellationToken ct = default);
}
