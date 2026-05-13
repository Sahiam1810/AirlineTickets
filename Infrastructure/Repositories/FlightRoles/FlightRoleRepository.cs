using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightRoles;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.FlightRoles;

public sealed class FlightRoleRepository : IFlightRoleRepository
{
    private readonly AppDbContext _context;

    public FlightRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FlightRole?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<FlightRole>()
            .AsTracking()
            .FirstOrDefaultAsync(fr => fr.Id == id, ct);

    public Task<IReadOnlyList<FlightRole>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<FlightRole>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<FlightRole>)t.Result, ct);

    public async Task<IReadOnlyList<FlightRole>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<FlightRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightRoles
                .Where(fr => EF.Functions.ILike(fr.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(fr => fr.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<FlightRole> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightRoles.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightRoles
                .Where(fr => EF.Functions.ILike(fr.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(FlightRole flightRole, CancellationToken ct = default)
    {
        _context.FlightRoles.Add(flightRole);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FlightRole flightRole, CancellationToken ct = default)
    {
        _context.FlightRoles.Update(flightRole);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FlightRole flightRole, CancellationToken ct = default)
    {
        _context.FlightRoles.Remove(flightRole);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(FlightRoleName name, CancellationToken ct = default) =>
        _context.FlightRoles
            .AsNoTracking()
            .AnyAsync(fr => EF.Functions.ILike(fr.Name.Value, name.Value), ct);
}
