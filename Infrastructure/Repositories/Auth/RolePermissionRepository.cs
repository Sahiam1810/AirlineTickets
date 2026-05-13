using Application.Abstractions;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class RolePermissionRepository : IRolePermissionRepository
{
    private readonly AppDbContext _context;

    public RolePermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RolePermission?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.RolePermissions
            .AsTracking()
            .Include(rp => rp.SystemRole)
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.Id == id, ct);

    public Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken ct = default) =>
        _context.RolePermissions
            .AsNoTracking()
            .Include(rp => rp.SystemRole)
            .Include(rp => rp.Permission)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<RolePermission>)t.Result, ct);

    public async Task<IReadOnlyList<RolePermission>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        IQueryable<RolePermission> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RolePermissions
                .Include(rp => rp.SystemRole)
                .Include(rp => rp.Permission)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RolePermissions
                .Include(rp => rp.SystemRole)
                .Include(rp => rp.Permission)
                .Where(rp => EF.Functions.ILike(rp.SystemRole.Name.Value, pattern)
                    || EF.Functions.ILike(rp.Permission.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rp => rp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<RolePermission> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RolePermissions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RolePermissions
                .Where(rp => EF.Functions.ILike(rp.SystemRole.Name.Value, pattern)
                    || EF.Functions.ILike(rp.Permission.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(RolePermission rolePermission, CancellationToken ct = default)
    {
        _context.RolePermissions.Add(rolePermission);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(RolePermission rolePermission, CancellationToken ct = default)
    {
        _context.RolePermissions.Update(rolePermission);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(RolePermission rolePermission, CancellationToken ct = default)
    {
        _context.RolePermissions.Remove(rolePermission);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int roleId, int permissionId, CancellationToken ct = default) =>
        _context.RolePermissions
            .AsNoTracking()
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, ct);
}
