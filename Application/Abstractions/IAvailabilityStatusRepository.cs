using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;

namespace Application.Abstractions;

public interface IAvailabilityStatusRepository
{
    Task<AvailabilityStatus?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AvailabilityStatus>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AvailabilityStatus>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default);
    Task UpdateAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default);
    Task RemoveAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(AvailabilityStatusName name, CancellationToken ct = default);
}
