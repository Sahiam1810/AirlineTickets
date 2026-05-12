using Domain.Entities.Staff;

namespace Application.Abstractions;

public interface IStaffAvailabilityRepository
{
    Task<StaffAvailability?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<StaffAvailability>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StaffAvailability>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(StaffAvailability staffAvailability, CancellationToken ct = default);
    Task UpdateAsync(StaffAvailability staffAvailability, CancellationToken ct = default);
    Task RemoveAsync(StaffAvailability staffAvailability, CancellationToken ct = default);
    Task<bool> ExistsOverlappingAsync(int staffId, DateTime startDate, DateTime endDate, int? excludedId = null, CancellationToken ct = default);
}
