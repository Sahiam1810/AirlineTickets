using Application.Abstractions;
using Domain.Entities.Reservations;
using Domain.ValueObjects.ReservationStatuses;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ReservationStatuses;

public sealed class ReservationStatusRepository : IReservationStatusRepository
{
    private readonly AppDbContext _context;

    public ReservationStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ReservationStatus?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<ReservationStatus>()
            .AsTracking()
            .FirstOrDefaultAsync(rs => rs.Id == id, ct);

    public Task<IReadOnlyList<ReservationStatus>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<ReservationStatus>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<ReservationStatus>)t.Result, ct);

    public async Task<IReadOnlyList<ReservationStatus>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<ReservationStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.ReservationStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.ReservationStatuses
                .Where(rs => EF.Functions.ILike(rs.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rs => rs.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<ReservationStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.ReservationStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.ReservationStatuses
                .Where(rs => EF.Functions.ILike(rs.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(ReservationStatus reservationStatus, CancellationToken ct = default)
    {
        _context.ReservationStatuses.Add(reservationStatus);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ReservationStatus reservationStatus, CancellationToken ct = default)
    {
        _context.ReservationStatuses.Update(reservationStatus);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ReservationStatus reservationStatus, CancellationToken ct = default)
    {
        _context.ReservationStatuses.Remove(reservationStatus);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(ReservationStatusName name, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.ReservationStatuses
            .AsNoTracking()
            .Where(rs => EF.Functions.ILike(rs.Name.Value, name.Value));

        if (excludedId.HasValue)
        {
            query = query.Where(rs => rs.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
