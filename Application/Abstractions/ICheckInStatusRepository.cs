using Domain.Entities.Tickets;

namespace Application.Abstractions;

public interface ICheckInStatusRepository
{
    Task<CheckInStatus?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CheckInStatus>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CheckInStatus>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CheckInStatus checkInStatus, CancellationToken ct = default);
    Task UpdateAsync(CheckInStatus checkInStatus, CancellationToken ct = default);
    Task RemoveAsync(CheckInStatus checkInStatus, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, int? excludedId = null, CancellationToken ct = default);
}
