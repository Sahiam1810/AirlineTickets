using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CabinTypes;

public sealed class CabinTypeRepository : ICabinTypeRepository
{
    private readonly AppDbContext _context;

    public CabinTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CabinType?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<CabinType>()
            .AsTracking()
            .FirstOrDefaultAsync(ct => ct.Id == id, ct);

    public Task<IReadOnlyList<CabinType>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<CabinType>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<CabinType>)t.Result, ct);

    public async Task<IReadOnlyList<CabinType>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<CabinType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CabinTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CabinTypes
                .Where(ct => EF.Functions.ILike(ct.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(ct => ct.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<CabinType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CabinTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CabinTypes
                .Where(ct => EF.Functions.ILike(ct.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(CabinType cabinType, CancellationToken ct = default)
    {
        _context.CabinTypes.Add(cabinType);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CabinType cabinType, CancellationToken ct = default)
    {
        _context.CabinTypes.Update(cabinType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CabinType cabinType, CancellationToken ct = default)
    {
        _context.CabinTypes.Remove(cabinType);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(CabinTypeName name, CancellationToken ct = default) =>
        _context.CabinTypes
            .AsNoTracking()
            .AnyAsync(ct => EF.Functions.ILike(ct.Name.Value, name.Value), ct);
}
