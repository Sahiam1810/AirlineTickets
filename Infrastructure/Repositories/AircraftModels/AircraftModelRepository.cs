using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AircraftModels;

public sealed class AircraftModelRepository : IAircraftModelRepository
{
    private readonly AppDbContext _context;

    public AircraftModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<AircraftModel?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<AircraftModel>()
            .Include(am => am.Manufacturer)
            .AsTracking()
            .FirstOrDefaultAsync(am => am.Id == id, ct);

    public Task<IReadOnlyList<AircraftModel>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<AircraftModel>()
            .Include(am => am.Manufacturer)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<AircraftModel>)t.Result, ct);

    public async Task<IReadOnlyList<AircraftModel>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<AircraftModel> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AircraftModels
                .Include(am => am.Manufacturer)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AircraftModels
                .Include(am => am.Manufacturer)
                .Where(am =>
                    EF.Functions.ILike(am.ModelName.Value, pattern) ||
                    EF.Functions.ILike(am.Manufacturer.Name.Value, pattern) ||
                    EF.Functions.ILike(am.Manufacturer.Country.Value, pattern))
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
        IQueryable<AircraftModel> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.AircraftModels.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.AircraftModels
                .Where(am =>
                    EF.Functions.ILike(am.ModelName.Value, pattern) ||
                    EF.Functions.ILike(am.Manufacturer.Name.Value, pattern) ||
                    EF.Functions.ILike(am.Manufacturer.Country.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(AircraftModel aircraftModel, CancellationToken ct = default)
    {
        _context.AircraftModels.Add(aircraftModel);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(AircraftModel aircraftModel, CancellationToken ct = default)
    {
        _context.AircraftModels.Update(aircraftModel);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AircraftModel aircraftModel, CancellationToken ct = default)
    {
        _context.AircraftModels.Remove(aircraftModel);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(int manufacturerId, ModelName modelName, CancellationToken ct = default) =>
        _context.AircraftModels
            .AsNoTracking()
            .AnyAsync(am =>
                am.ManufacturerId == manufacturerId &&
                EF.Functions.ILike(am.ModelName.Value, modelName.Value), ct);
}
