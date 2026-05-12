using Domain.Entities.Flights;
using Domain.ValueObjects.FlightStates;

namespace Application.Abstractions;

public interface IFlightStateRepository
{
    Task<FlightState?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FlightState>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FlightState>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(FlightState flightState, CancellationToken ct = default);
    Task UpdateAsync(FlightState flightState, CancellationToken ct = default);
    Task RemoveAsync(FlightState flightState, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(FlightStateName name, CancellationToken ct = default);
}
