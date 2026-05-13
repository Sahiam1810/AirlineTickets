using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Passengers;

public sealed class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Passenger?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Passenger>())
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Passenger?> GetByPersonIdAsync(int personId, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Passenger>())
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PersonId == personId, ct);

    public Task<IReadOnlyList<Passenger>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Passenger>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Passenger>)t.Result, ct);

    public async Task<IReadOnlyList<Passenger>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Passenger> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.Passengers)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.Passengers)
                .Where(p =>
                    EF.Functions.ILike(p.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(p.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(p.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(p.PassengerType.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Passenger> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Passengers.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Passengers
                .Where(p =>
                    EF.Functions.ILike(p.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(p.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(p.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(p.PassengerType.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Passenger passenger, CancellationToken ct = default)
    {
        _context.Passengers.Add(passenger);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Passenger passenger, CancellationToken ct = default)
    {
        _context.Passengers.Update(passenger);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Passenger passenger, CancellationToken ct = default)
    {
        _context.Passengers.Remove(passenger);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByPersonIdAsync(int personId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.Passengers
            .AsNoTracking()
            .Where(p => p.PersonId == personId);

        if (excludedId.HasValue)
        {
            query = query.Where(p => p.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<Passenger> IncludeDetails(IQueryable<Passenger> query) =>
        query
            .Include(p => p.Person)
            .Include(p => p.PassengerType);
}
