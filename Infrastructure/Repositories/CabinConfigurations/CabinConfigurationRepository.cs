using Application.Abstractions;
using Domain.Entities.Aircraft;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CabinConfigurations;

public sealed class CabinConfigurationRepository : ICabinConfigurationRepository
{
    private readonly AppDbContext _context;

    public CabinConfigurationRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CabinConfiguration?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<CabinConfiguration>()
            .Include(cc => cc.Aircraft)
                .ThenInclude(a => a.AircraftModel)
            .Include(cc => cc.Aircraft)
                .ThenInclude(a => a.Airline)
            .Include(cc => cc.CabinType)
            .AsTracking()
            .FirstOrDefaultAsync(cc => cc.Id == id, ct);

    public Task<IReadOnlyList<CabinConfiguration>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<CabinConfiguration>()
            .Include(cc => cc.Aircraft)
                .ThenInclude(a => a.AircraftModel)
            .Include(cc => cc.Aircraft)
                .ThenInclude(a => a.Airline)
            .Include(cc => cc.CabinType)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<CabinConfiguration>)t.Result, ct);

    public async Task<IReadOnlyList<CabinConfiguration>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<CabinConfiguration> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CabinConfigurations
                .Include(cc => cc.Aircraft)
                    .ThenInclude(a => a.AircraftModel)
                .Include(cc => cc.Aircraft)
                    .ThenInclude(a => a.Airline)
                .Include(cc => cc.CabinType)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CabinConfigurations
                .Include(cc => cc.Aircraft)
                    .ThenInclude(a => a.AircraftModel)
                .Include(cc => cc.Aircraft)
                    .ThenInclude(a => a.Airline)
                .Include(cc => cc.CabinType)
                .Where(cc =>
                    EF.Functions.ILike(cc.Aircraft.Registration.Value, pattern) ||
                    EF.Functions.ILike(cc.Aircraft.AircraftModel.ModelName.Value, pattern) ||
                    EF.Functions.ILike(cc.Aircraft.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(cc.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(cc.SeatLetters.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(cc => cc.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<CabinConfiguration> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CabinConfigurations.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CabinConfigurations
                .Where(cc =>
                    EF.Functions.ILike(cc.Aircraft.Registration.Value, pattern) ||
                    EF.Functions.ILike(cc.Aircraft.AircraftModel.ModelName.Value, pattern) ||
                    EF.Functions.ILike(cc.Aircraft.Airline.Name.Value, pattern) ||
                    EF.Functions.ILike(cc.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(cc.SeatLetters.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default)
    {
        _context.CabinConfigurations.Add(cabinConfiguration);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default)
    {
        _context.CabinConfigurations.Update(cabinConfiguration);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default)
    {
        _context.CabinConfigurations.Remove(cabinConfiguration);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int aircraftId, int cabinTypeId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.CabinConfigurations
            .AsNoTracking()
            .Where(cc => cc.AircraftId == aircraftId && cc.CabinTypeId == cabinTypeId);

        if (excludedId.HasValue)
        {
            query = query.Where(cc => cc.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
