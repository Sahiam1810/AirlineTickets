using Domain.Entities.Auth;

namespace Application.Abstractions;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Permission>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Permission permission, CancellationToken ct = default);
    Task UpdateAsync(Permission permission, CancellationToken ct = default);
    Task RemoveAsync(Permission permission, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
