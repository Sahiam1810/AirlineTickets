using Application.Abstractions;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AvailabilityStatuses;

public sealed class AvailabilityStatusRepository : IAvailabilityStatusRepository
{
    private readonly AppDbContext _context;

    public AvailabilityStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<AvailabilityStatus?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<AvailabilityStatus>()
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<IReadOnlyList<AvailabilityStatus>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<AvailabilityStatus>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<AvailabilityStatus>)t.Result, ct);

    public async Task<IReadOnlyList<AvailabilityStatus>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<AvailabilityStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AvailabilityStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AvailabilityStatuses
                .Where(a => EF.Functions.ILike(a.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<AvailabilityStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AvailabilityStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AvailabilityStatuses
                .Where(a => EF.Functions.ILike(a.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default)
    {
        _context.AvailabilityStatuses.Add(availabilityStatus);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default)
    {
        _context.AvailabilityStatuses.Update(availabilityStatus);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AvailabilityStatus availabilityStatus, CancellationToken ct = default)
    {
        _context.AvailabilityStatuses.Remove(availabilityStatus);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(AvailabilityStatusName name, CancellationToken ct = default) =>
        _context.AvailabilityStatuses
            .AsNoTracking()
            .AnyAsync(a => EF.Functions.ILike(a.Name.Value, name.Value), ct);
}
