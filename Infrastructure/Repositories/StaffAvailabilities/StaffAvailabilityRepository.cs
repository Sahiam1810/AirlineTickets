using Application.Abstractions;
using Domain.Entities.Staff;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.StaffAvailabilities;

public sealed class StaffAvailabilityRepository : IStaffAvailabilityRepository
{
    private readonly AppDbContext _context;

    public StaffAvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<StaffAvailability?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<StaffAvailability>()
            .Include(sa => sa.Staff)
                .ThenInclude(s => s.Person)
            .Include(sa => sa.AvailabilityStatus)
            .AsTracking()
            .FirstOrDefaultAsync(sa => sa.Id == id, ct);

    public Task<IReadOnlyList<StaffAvailability>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<StaffAvailability>()
            .Include(sa => sa.Staff)
                .ThenInclude(s => s.Person)
            .Include(sa => sa.AvailabilityStatus)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<StaffAvailability>)t.Result, ct);

    public async Task<IReadOnlyList<StaffAvailability>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<StaffAvailability> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.StaffAvailabilities
                .Include(sa => sa.Staff)
                    .ThenInclude(s => s.Person)
                .Include(sa => sa.AvailabilityStatus)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.StaffAvailabilities
                .Include(sa => sa.Staff)
                    .ThenInclude(s => s.Person)
                .Include(sa => sa.AvailabilityStatus)
                .Where(sa =>
                    EF.Functions.ILike(sa.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(sa.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(sa.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(sa.AvailabilityStatus.Name.Value, pattern) ||
                    (sa.Notes != null && EF.Functions.ILike(sa.Notes.Value, pattern)))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(sa => sa.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<StaffAvailability> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.StaffAvailabilities.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.StaffAvailabilities
                .Where(sa =>
                    EF.Functions.ILike(sa.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(sa.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(sa.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(sa.AvailabilityStatus.Name.Value, pattern) ||
                    (sa.Notes != null && EF.Functions.ILike(sa.Notes.Value, pattern)))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(StaffAvailability staffAvailability, CancellationToken ct = default)
    {
        _context.StaffAvailabilities.Add(staffAvailability);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(StaffAvailability staffAvailability, CancellationToken ct = default)
    {
        _context.StaffAvailabilities.Update(staffAvailability);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(StaffAvailability staffAvailability, CancellationToken ct = default)
    {
        _context.StaffAvailabilities.Remove(staffAvailability);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsOverlappingAsync(
        int staffId,
        DateTime startDate,
        DateTime endDate,
        int? excludedId = null,
        CancellationToken ct = default)
    {
        var query = _context.StaffAvailabilities
            .AsNoTracking()
            .Where(sa =>
                sa.StaffId == staffId &&
                startDate < sa.EndDate &&
                endDate > sa.StartDate);

        if (excludedId.HasValue)
        {
            query = query.Where(sa => sa.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
