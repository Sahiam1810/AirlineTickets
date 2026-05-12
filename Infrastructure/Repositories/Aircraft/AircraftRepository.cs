using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Aircraft;

public sealed class AircraftRepository : IAircraftRepository
{
    private readonly AppDbContext _context;

    public AircraftRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<AircraftUnit?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<AircraftUnit>()
            .Include(a => a.AircraftModel)
                .ThenInclude(m => m.Manufacturer)
            .Include(a => a.Airline)
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<IReadOnlyList<AircraftUnit>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<AircraftUnit>()
            .Include(a => a.AircraftModel)
                .ThenInclude(m => m.Manufacturer)
            .Include(a => a.Airline)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<AircraftUnit>)t.Result, ct);

    public async Task<IReadOnlyList<AircraftUnit>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<AircraftUnit> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Aircraft
                .Include(a => a.AircraftModel)
                    .ThenInclude(m => m.Manufacturer)
                .Include(a => a.Airline)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Aircraft
                .Include(a => a.AircraftModel)
                    .ThenInclude(m => m.Manufacturer)
                .Include(a => a.Airline)
                .Where(a =>
                    EF.Functions.ILike(a.Registration.Value, pattern) ||
                    EF.Functions.ILike(a.AircraftModel.ModelName.Value, pattern) ||
                    EF.Functions.ILike(a.AircraftModel.Manufacturer.Name.Value, pattern) ||
                    EF.Functions.ILike(a.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(a.Airline.IataCode.Value, pattern))
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
        IQueryable<AircraftUnit> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Aircraft.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Aircraft
                .Where(a =>
                    EF.Functions.ILike(a.Registration.Value, pattern) ||
                    EF.Functions.ILike(a.AircraftModel.ModelName.Value, pattern) ||
                    EF.Functions.ILike(a.AircraftModel.Manufacturer.Name.Value, pattern) ||
                    EF.Functions.ILike(a.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(a.Airline.IataCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(AircraftUnit aircraft, CancellationToken ct = default)
    {
        _context.Aircraft.Add(aircraft);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(AircraftUnit aircraft, CancellationToken ct = default)
    {
        _context.Aircraft.Update(aircraft);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AircraftUnit aircraft, CancellationToken ct = default)
    {
        _context.Aircraft.Remove(aircraft);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByRegistrationAsync(Registration registration, CancellationToken ct = default) =>
        _context.Aircraft
            .AsNoTracking()
            .AnyAsync(a => EF.Functions.ILike(a.Registration.Value, registration.Value), ct);
}
