using Application.Abstractions;
using Domain.Entities.Staff;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.StaffRoles;

public sealed class StaffRoleRepository : IStaffRoleRepository
{
    private readonly AppDbContext _context;

    public StaffRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<StaffRole?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<StaffRole>()
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<IReadOnlyList<StaffRole>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<StaffRole>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<StaffRole>)t.Result, ct);

    public async Task<IReadOnlyList<StaffRole>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<StaffRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.StaffRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.StaffRoles
                .Where(s => EF.Functions.ILike(s.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<StaffRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.StaffRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.StaffRoles
                .Where(s => EF.Functions.ILike(s.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(StaffRole staffRole, CancellationToken ct = default)
    {
        _context.StaffRoles.Add(staffRole);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(StaffRole staffRole, CancellationToken ct = default)
    {
        _context.StaffRoles.Update(staffRole);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(StaffRole staffRole, CancellationToken ct = default)
    {
        _context.StaffRoles.Remove(staffRole);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.StaffRoles
            .AsNoTracking()
            .AnyAsync(s => EF.Functions.ILike(s.Name.Value, pattern), ct);
    }
}
