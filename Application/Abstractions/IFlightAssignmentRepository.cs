using Domain.Entities.Flights;

namespace Application.Abstractions;

public interface IFlightAssignmentRepository
{
    Task<FlightAssignment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FlightAssignment>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FlightAssignment>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(FlightAssignment flightAssignment, CancellationToken ct = default);
    Task UpdateAsync(FlightAssignment flightAssignment, CancellationToken ct = default);
    Task RemoveAsync(FlightAssignment flightAssignment, CancellationToken ct = default);
    Task<bool> ExistsAsync(int flightId, int staffId, int? excludedId = null, CancellationToken ct = default);
}
