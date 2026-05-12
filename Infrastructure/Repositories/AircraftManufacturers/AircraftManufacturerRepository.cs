using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AircraftManufacturers;

public sealed class AircraftManufacturerRepository : IAircraftManufacturerRepository
{
    private readonly AppDbContext _context;

    public AircraftManufacturerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<AircraftManufacturer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<AircraftManufacturer>()
            .AsTracking()
            .FirstOrDefaultAsync(am => am.Id == id, ct);

    public Task<IReadOnlyList<AircraftManufacturer>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<AircraftManufacturer>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<AircraftManufacturer>)t.Result, ct);

    public async Task<IReadOnlyList<AircraftManufacturer>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<AircraftManufacturer> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AircraftManufacturers.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AircraftManufacturers
                .Where(am =>
                    EF.Functions.ILike(am.Name.Value, pattern) ||
                    EF.Functions.ILike(am.Country.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(am => am.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<AircraftManufacturer> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AircraftManufacturers.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AircraftManufacturers
                .Where(am =>
                    EF.Functions.ILike(am.Name.Value, pattern) ||
                    EF.Functions.ILike(am.Country.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default)
    {
        _context.AircraftManufacturers.Add(aircraftManufacturer);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default)
    {
        _context.AircraftManufacturers.Update(aircraftManufacturer);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default)
    {
        _context.AircraftManufacturers.Remove(aircraftManufacturer);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(ManufacturerName name, CancellationToken ct = default) =>
        _context.AircraftManufacturers
            .AsNoTracking()
            .AnyAsync(am => EF.Functions.ILike(am.Name.Value, name.Value), ct);
}
