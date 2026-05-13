using Domain.Entities.Flights;
using Domain.ValueObjects.FlightRoles;

namespace Application.Abstractions;

public interface IFlightRoleRepository
{
    Task<FlightRole?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FlightRole>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FlightRole>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(FlightRole flightRole, CancellationToken ct = default);
    Task UpdateAsync(FlightRole flightRole, CancellationToken ct = default);
    Task RemoveAsync(FlightRole flightRole, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(FlightRoleName name, CancellationToken ct = default);
}
