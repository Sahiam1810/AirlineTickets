using Application.Abstractions;
using Domain.Entities.Flights;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.FlightStatusTransitions;

public sealed class FlightStatusTransitionRepository : IFlightStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public FlightStatusTransitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FlightStatusTransition?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightStatusTransition>())
            .AsTracking()
            .FirstOrDefaultAsync(fst => fst.Id == id, ct);

    public Task<IReadOnlyList<FlightStatusTransition>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightStatusTransition>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<FlightStatusTransition>)t.Result, ct);

    public async Task<IReadOnlyList<FlightStatusTransition>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<FlightStatusTransition> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.FlightStatusTransitions)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.FlightStatusTransitions)
                .Where(fst =>
                    EF.Functions.ILike(fst.FromState.Name.Value, pattern) ||
                    EF.Functions.ILike(fst.ToState.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(fst => fst.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<FlightStatusTransition> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightStatusTransitions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightStatusTransitions
                .Where(fst =>
                    EF.Functions.ILike(fst.FromState.Name.Value, pattern) ||
                    EF.Functions.ILike(fst.ToState.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default)
    {
        _context.FlightStatusTransitions.Add(flightStatusTransition);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default)
    {
        _context.FlightStatusTransitions.Update(flightStatusTransition);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FlightStatusTransition flightStatusTransition, CancellationToken ct = default)
    {
        _context.FlightStatusTransitions.Remove(flightStatusTransition);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(
        int fromStateId,
        int toStateId,
        int? excludedId = null,
        CancellationToken ct = default)
    {
        var query = _context.FlightStatusTransitions
            .AsNoTracking()
            .Where(fst => fst.FromStateId == fromStateId && fst.ToStateId == toStateId);

        if (excludedId.HasValue)
        {
            query = query.Where(fst => fst.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<FlightStatusTransition> IncludeDetails(IQueryable<FlightStatusTransition> query) =>
        query
            .Include(fst => fst.FromState)
            .Include(fst => fst.ToState);
}
