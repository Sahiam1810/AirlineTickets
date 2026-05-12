using Domain.Entities.Airlines;

namespace Application.Abstractions;

public interface IAirportAirlineRepository
{
    Task<AirportAirline?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AirportAirline>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AirportAirline>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(AirportAirline airportAirline, CancellationToken ct = default);
    Task UpdateAsync(AirportAirline airportAirline, CancellationToken ct = default);
    Task RemoveAsync(AirportAirline airportAirline, CancellationToken ct = default);
    Task<bool> ExistsAsync(int airportId, int airlineId, CancellationToken ct = default);
}
