using Application.Abstractions;
using Domain.Entities.Reservations;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ReservationStatusTransitions;

public sealed class ReservationStatusTransitionRepository : IReservationStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public ReservationStatusTransitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ReservationStatusTransition?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationStatusTransition>())
            .AsTracking()
            .FirstOrDefaultAsync(rst => rst.Id == id, ct);

    public Task<IReadOnlyList<ReservationStatusTransition>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationStatusTransition>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<ReservationStatusTransition>)t.Result, ct);

    public async Task<IReadOnlyList<ReservationStatusTransition>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<ReservationStatusTransition> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.ReservationStatusTransitions)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.ReservationStatusTransitions)
                .Where(rst =>
                    EF.Functions.ILike(rst.FromStatus.Name.Value, pattern) ||
                    EF.Functions.ILike(rst.ToStatus.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rst => rst.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<ReservationStatusTransition> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.ReservationStatusTransitions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.ReservationStatusTransitions
                .Where(rst =>
                    EF.Functions.ILike(rst.FromStatus.Name.Value, pattern) ||
                    EF.Functions.ILike(rst.ToStatus.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default)
    {
        _context.ReservationStatusTransitions.Add(reservationStatusTransition);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default)
    {
        _context.ReservationStatusTransitions.Update(reservationStatusTransition);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default)
    {
        _context.ReservationStatusTransitions.Remove(reservationStatusTransition);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int fromStatusId, int toStatusId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.ReservationStatusTransitions
            .AsNoTracking()
            .Where(rst => rst.FromStatusId == fromStatusId && rst.ToStatusId == toStatusId);

        if (excludedId.HasValue)
        {
            query = query.Where(rst => rst.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<ReservationStatusTransition> IncludeDetails(IQueryable<ReservationStatusTransition> query) =>
        query
            .Include(rst => rst.FromStatus)
            .Include(rst => rst.ToStatus);
}
