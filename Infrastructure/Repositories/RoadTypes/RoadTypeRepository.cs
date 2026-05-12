using Application.Abstractions;
using Domain.Entities.Location;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RoadTypes;

public sealed class RoadTypeRepository : IRoadTypeRepository
{
    private readonly AppDbContext _context;

    public RoadTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RoadType?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<RoadType>()
            .AsTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<IReadOnlyList<RoadType>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<RoadType>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<RoadType>)t.Result, ct);

    public async Task<IReadOnlyList<RoadType>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<RoadType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RoadTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RoadTypes
                .Where(r => EF.Functions.ILike(r.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<RoadType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.RoadTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.RoadTypes
                .Where(r => EF.Functions.ILike(r.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(RoadType roadType, CancellationToken ct = default)
    {
        _context.RoadTypes.Add(roadType);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(RoadType roadType, CancellationToken ct = default)
    {
        _context.RoadTypes.Update(roadType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(RoadType roadType, CancellationToken ct = default)
    {
        _context.RoadTypes.Remove(roadType);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.RoadTypes
            .AsNoTracking()
            .AnyAsync(r => EF.Functions.ILike(r.Name.Value, pattern), ct);
    }
}
