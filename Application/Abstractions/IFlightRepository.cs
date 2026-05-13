using Domain.Entities.Flights;
using Domain.ValueObjects.Flights;

namespace Application.Abstractions;

public interface IFlightRepository
{
    Task<Flight?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Flight?> GetByCodeAsync(FlightCode flightCode, CancellationToken ct = default);
    Task<IReadOnlyList<Flight>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Flight>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Flight flight, CancellationToken ct = default);
    Task UpdateAsync(Flight flight, CancellationToken ct = default);
    Task RemoveAsync(Flight flight, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(FlightCode flightCode, int? excludedId = null, CancellationToken ct = default);
}
