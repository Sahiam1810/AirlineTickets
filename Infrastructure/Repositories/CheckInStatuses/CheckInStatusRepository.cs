using Application.Abstractions;
using Domain.Entities.Tickets;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CheckInStatuses;

public sealed class CheckInStatusRepository : ICheckInStatusRepository
{
    private readonly AppDbContext _context;

    public CheckInStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CheckInStatus?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<CheckInStatus>()
            .AsTracking()
            .FirstOrDefaultAsync(cs => cs.Id == id, ct);

    public Task<IReadOnlyList<CheckInStatus>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<CheckInStatus>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<CheckInStatus>)t.Result, ct);

    public async Task<IReadOnlyList<CheckInStatus>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<CheckInStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CheckInStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CheckInStatuses
                .Where(cs => EF.Functions.ILike(cs.Name, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(cs => cs.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<CheckInStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CheckInStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CheckInStatuses
                .Where(cs => EF.Functions.ILike(cs.Name, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(CheckInStatus checkInStatus, CancellationToken ct = default)
    {
        _context.CheckInStatuses.Add(checkInStatus);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CheckInStatus checkInStatus, CancellationToken ct = default)
    {
        _context.CheckInStatuses.Update(checkInStatus);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CheckInStatus checkInStatus, CancellationToken ct = default)
    {
        _context.CheckInStatuses.Remove(checkInStatus);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.CheckInStatuses
            .AsNoTracking()
            .Where(cs => EF.Functions.ILike(cs.Name, name));

        if (excludedId.HasValue)
        {
            query = query.Where(cs => cs.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
