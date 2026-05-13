using Application.Abstractions;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class SystemRoleRepository : ISystemRoleRepository
{
    private readonly AppDbContext _context;

    public SystemRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<SystemRole?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.SystemRoles
            .AsTracking()
            .FirstOrDefaultAsync(sr => sr.Id == id, ct);

    public Task<IReadOnlyList<SystemRole>> GetAllAsync(CancellationToken ct = default) =>
        _context.SystemRoles
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<SystemRole>)t.Result, ct);

    public async Task<IReadOnlyList<SystemRole>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        IQueryable<SystemRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.SystemRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.SystemRoles
                .Where(sr => EF.Functions.ILike(sr.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(sr => sr.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<SystemRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.SystemRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.SystemRoles
                .Where(sr => EF.Functions.ILike(sr.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(SystemRole systemRole, CancellationToken ct = default)
    {
        _context.SystemRoles.Add(systemRole);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SystemRole systemRole, CancellationToken ct = default)
    {
        _context.SystemRoles.Update(systemRole);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(SystemRole systemRole, CancellationToken ct = default)
    {
        _context.SystemRoles.Remove(systemRole);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var trimmed = name.Trim();

        return _context.SystemRoles
            .AsNoTracking()
            .AnyAsync(sr => EF.Functions.ILike(sr.Name.Value, trimmed), ct);
    }
}
