using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.SeatLocationTypes;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.SeatLocationTypes;

public sealed class SeatLocationTypeRepository : ISeatLocationTypeRepository
{
    private readonly AppDbContext _context;

    public SeatLocationTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<SeatLocationType?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<SeatLocationType>()
            .AsTracking()
            .FirstOrDefaultAsync(slt => slt.Id == id, ct);

    public Task<IReadOnlyList<SeatLocationType>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<SeatLocationType>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<SeatLocationType>)t.Result, ct);

    public async Task<IReadOnlyList<SeatLocationType>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<SeatLocationType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.SeatLocationTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.SeatLocationTypes
                .Where(slt => EF.Functions.ILike(slt.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(slt => slt.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<SeatLocationType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.SeatLocationTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.SeatLocationTypes
                .Where(slt => EF.Functions.ILike(slt.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(SeatLocationType seatLocationType, CancellationToken ct = default)
    {
        _context.SeatLocationTypes.Add(seatLocationType);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SeatLocationType seatLocationType, CancellationToken ct = default)
    {
        _context.SeatLocationTypes.Update(seatLocationType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(SeatLocationType seatLocationType, CancellationToken ct = default)
    {
        _context.SeatLocationTypes.Remove(seatLocationType);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(SeatLocationTypeName name, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.SeatLocationTypes
            .AsNoTracking()
            .Where(slt => EF.Functions.ILike(slt.Name.Value, name.Value));

        if (excludedId.HasValue)
        {
            query = query.Where(slt => slt.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
