using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;

namespace Application.Abstractions;

public interface IAircraftRepository
{
    Task<AircraftUnit?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AircraftUnit>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AircraftUnit>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(AircraftUnit aircraft, CancellationToken ct = default);
    Task UpdateAsync(AircraftUnit aircraft, CancellationToken ct = default);
    Task RemoveAsync(AircraftUnit aircraft, CancellationToken ct = default);
    Task<bool> ExistsByRegistrationAsync(Registration registration, CancellationToken ct = default);
}
