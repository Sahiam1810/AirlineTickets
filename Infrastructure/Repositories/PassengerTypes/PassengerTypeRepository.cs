using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.PassengerTypes;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PassengerTypes;

public sealed class PassengerTypeRepository : IPassengerTypeRepository
{
    private readonly AppDbContext _context;

    public PassengerTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<PassengerType?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<PassengerType>()
            .AsTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id, ct);

    public Task<IReadOnlyList<PassengerType>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<PassengerType>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<PassengerType>)t.Result, ct);

    public async Task<IReadOnlyList<PassengerType>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<PassengerType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PassengerTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PassengerTypes
                .Where(pt => EF.Functions.ILike(pt.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(pt => pt.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<PassengerType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PassengerTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PassengerTypes
                .Where(pt => EF.Functions.ILike(pt.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(PassengerType passengerType, CancellationToken ct = default)
    {
        _context.PassengerTypes.Add(passengerType);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PassengerType passengerType, CancellationToken ct = default)
    {
        _context.PassengerTypes.Update(passengerType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PassengerType passengerType, CancellationToken ct = default)
    {
        _context.PassengerTypes.Remove(passengerType);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(PassengerTypeName name, CancellationToken ct = default) =>
        _context.PassengerTypes
            .AsNoTracking()
            .AnyAsync(pt => EF.Functions.ILike(pt.Name.Value, name.Value), ct);
}
