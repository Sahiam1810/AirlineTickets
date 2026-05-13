using Domain.Entities.Auth;

namespace Application.Abstractions;

public interface ISystemRoleRepository
{
    Task<SystemRole?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SystemRole>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SystemRole>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(SystemRole systemRole, CancellationToken ct = default);
    Task UpdateAsync(SystemRole systemRole, CancellationToken ct = default);
    Task RemoveAsync(SystemRole systemRole, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
