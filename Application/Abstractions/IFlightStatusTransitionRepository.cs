using Domain.Entities.Flights;

namespace Application.Abstractions;

public interface IFlightStatusTransitionRepository
{
    Task<FlightStatusTransition?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FlightStatusTransition>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FlightStatusTransition>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default);
    Task UpdateAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default);
    Task RemoveAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default);
    Task<bool> ExistsAsync(int fromStateId, int toStateId, int? excludedId = null, CancellationToken ct = default);
}
