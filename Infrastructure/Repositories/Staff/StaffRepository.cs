using Application.Abstractions;
using Domain.Entities.Staff;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Staff;

public sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<StaffMember?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<StaffMember>()
            .Include(s => s.Person)
            .Include(s => s.StaffRole)
            .Include(s => s.Airline)
            .Include(s => s.Airport)
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<IReadOnlyList<StaffMember>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<StaffMember>()
            .Include(s => s.Person)
            .Include(s => s.StaffRole)
            .Include(s => s.Airline)
            .Include(s => s.Airport)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<StaffMember>)t.Result, ct);

    public async Task<IReadOnlyList<StaffMember>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<StaffMember> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Staff
                .Include(s => s.Person)
                .Include(s => s.StaffRole)
                .Include(s => s.Airline)
                .Include(s => s.Airport)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Staff
                .Include(s => s.Person)
                .Include(s => s.StaffRole)
                .Include(s => s.Airline)
                .Include(s => s.Airport)
                .Where(s =>
                    EF.Functions.ILike(s.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(s.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(s.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(s.StaffRole.Name.Value, pattern) ||
                    (s.Airline != null && EF.Functions.ILike(s.Airline.Name.Value, pattern)) ||
                    (s.Airport != null && EF.Functions.ILike(s.Airport.Name.Value, pattern)))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<StaffMember> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Staff.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Staff
                .Where(s =>
                    EF.Functions.ILike(s.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(s.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(s.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(s.StaffRole.Name.Value, pattern) ||
                    (s.Airline != null && EF.Functions.ILike(s.Airline.Name.Value, pattern)) ||
                    (s.Airport != null && EF.Functions.ILike(s.Airport.Name.Value, pattern)))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(StaffMember staff, CancellationToken ct = default)
    {
        _context.Staff.Add(staff);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(StaffMember staff, CancellationToken ct = default)
    {
        _context.Staff.Update(staff);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(StaffMember staff, CancellationToken ct = default)
    {
        _context.Staff.Remove(staff);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByPersonIdAsync(int personId, CancellationToken ct = default) =>
        _context.Staff
            .AsNoTracking()
            .AnyAsync(s => s.PersonId == personId, ct);
}
