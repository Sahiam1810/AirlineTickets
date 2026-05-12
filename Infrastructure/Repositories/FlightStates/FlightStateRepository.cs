using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightStates;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.FlightStates;

public sealed class FlightStateRepository : IFlightStateRepository
{
    private readonly AppDbContext _context;

    public FlightStateRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FlightState?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<FlightState>()
            .AsTracking()
            .FirstOrDefaultAsync(fs => fs.Id == id, ct);

    public Task<IReadOnlyList<FlightState>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<FlightState>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<FlightState>)t.Result, ct);

    public async Task<IReadOnlyList<FlightState>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<FlightState> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightStates.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightStates
                .Where(fs => EF.Functions.ILike(fs.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(fs => fs.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<FlightState> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightStates.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightStates
                .Where(fs => EF.Functions.ILike(fs.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(FlightState flightState, CancellationToken ct = default)
    {
        _context.FlightStates.Add(flightState);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FlightState flightState, CancellationToken ct = default)
    {
        _context.FlightStates.Update(flightState);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FlightState flightState, CancellationToken ct = default)
    {
        _context.FlightStates.Remove(flightState);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(FlightStateName name, CancellationToken ct = default) =>
        _context.FlightStates
            .AsNoTracking()
            .AnyAsync(fs => EF.Functions.ILike(fs.Name.Value, name.Value), ct);
}
