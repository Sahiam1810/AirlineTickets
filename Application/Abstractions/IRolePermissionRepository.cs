using Domain.Entities.Auth;

namespace Application.Abstractions;

public interface IRolePermissionRepository
{
    Task<RolePermission?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RolePermission>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(RolePermission rolePermission, CancellationToken ct = default);
    Task UpdateAsync(RolePermission rolePermission, CancellationToken ct = default);
    Task RemoveAsync(RolePermission rolePermission, CancellationToken ct = default);
    Task<bool> ExistsAsync(int roleId, int permissionId, CancellationToken ct = default);
}
