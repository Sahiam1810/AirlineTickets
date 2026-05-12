using Domain.Entities.Staff;

namespace Application.Abstractions;

public interface IStaffRepository
{
    Task<StaffMember?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<StaffMember>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StaffMember>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(StaffMember staff, CancellationToken ct = default);
    Task UpdateAsync(StaffMember staff, CancellationToken ct = default);
    Task RemoveAsync(StaffMember staff, CancellationToken ct = default);
    Task<bool> ExistsByPersonIdAsync(int personId, CancellationToken ct = default);
}
