using Application.Abstractions;
using Domain.Entities.Flights;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.FlightAssignments;

public sealed class FlightAssignmentRepository : IFlightAssignmentRepository
{
    private readonly AppDbContext _context;

    public FlightAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FlightAssignment?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightAssignment>())
            .AsTracking()
            .FirstOrDefaultAsync(fa => fa.Id == id, ct);

    public Task<IReadOnlyList<FlightAssignment>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightAssignment>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<FlightAssignment>)t.Result, ct);

    public async Task<IReadOnlyList<FlightAssignment>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<FlightAssignment> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.FlightAssignments)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.FlightAssignments)
                .Where(fa =>
                    EF.Functions.ILike(fa.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(fa.FlightRole.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(fa => fa.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<FlightAssignment> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightAssignments.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightAssignments
                .Where(fa =>
                    EF.Functions.ILike(fa.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(fa.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(fa.FlightRole.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(FlightAssignment flightAssignment, CancellationToken ct = default)
    {
        _context.FlightAssignments.Add(flightAssignment);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FlightAssignment flightAssignment, CancellationToken ct = default)
    {
        _context.FlightAssignments.Update(flightAssignment);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FlightAssignment flightAssignment, CancellationToken ct = default)
    {
        _context.FlightAssignments.Remove(flightAssignment);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int flightId, int staffId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.FlightAssignments
            .AsNoTracking()
            .Where(fa => fa.FlightId == flightId && fa.StaffId == staffId);

        if (excludedId.HasValue)
        {
            query = query.Where(fa => fa.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<FlightAssignment> IncludeDetails(IQueryable<FlightAssignment> query) =>
        query
            .Include(fa => fa.Flight)
            .Include(fa => fa.Staff)
                .ThenInclude(s => s.Person)
            .Include(fa => fa.FlightRole);
}
