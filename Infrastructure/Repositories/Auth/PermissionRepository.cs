using Application.Abstractions;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Permission?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Permissions
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken ct = default) =>
        _context.Permissions
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Permission>)t.Result, ct);

    public async Task<IReadOnlyList<Permission>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        IQueryable<Permission> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Permissions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Permissions
                .Where(p => EF.Functions.ILike(p.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Permission> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Permissions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Permissions
                .Where(p => EF.Functions.ILike(p.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Permission permission, CancellationToken ct = default)
    {
        _context.Permissions.Add(permission);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Permission permission, CancellationToken ct = default)
    {
        _context.Permissions.Update(permission);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Permission permission, CancellationToken ct = default)
    {
        _context.Permissions.Remove(permission);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var trimmed = name.Trim();

        return _context.Permissions
            .AsNoTracking()
            .AnyAsync(p => EF.Functions.ILike(p.Name.Value, trimmed), ct);
    }
}
