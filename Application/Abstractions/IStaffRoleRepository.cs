using Domain.Entities.Staff;

namespace Application.Abstractions;

public interface IStaffRoleRepository
{
    Task<StaffRole?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<StaffRole>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StaffRole>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(StaffRole staffRole, CancellationToken ct = default);
    Task UpdateAsync(StaffRole staffRole, CancellationToken ct = default);
    Task RemoveAsync(StaffRole staffRole, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
