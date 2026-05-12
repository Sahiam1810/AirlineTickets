using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PersonPhones;

public sealed class PersonPhoneRepository : IPersonPhoneRepository
{
    private readonly AppDbContext _context;

    public PersonPhoneRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<PersonPhone?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<PersonPhone>()
            .Include(p => p.PhoneCode)
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<IReadOnlyList<PersonPhone>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<PersonPhone>()
            .Include(p => p.PhoneCode)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<PersonPhone>)t.Result, ct);

    public async Task<IReadOnlyList<PersonPhone>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<PersonPhone> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PersonPhones
                .Include(p => p.PhoneCode)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PersonPhones
                .Include(p => p.PhoneCode)
                .Where(p =>
                    EF.Functions.ILike(p.PhoneNumber.Value, pattern) ||
                    EF.Functions.ILike(p.PhoneCode.CountryCode.Value, pattern))
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
        IQueryable<PersonPhone> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PersonPhones.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PersonPhones
                .Where(p =>
                    EF.Functions.ILike(p.PhoneNumber.Value, pattern) ||
                    EF.Functions.ILike(p.PhoneCode.CountryCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(PersonPhone personPhone, CancellationToken ct = default)
    {
        _context.PersonPhones.Add(personPhone);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PersonPhone personPhone, CancellationToken ct = default)
    {
        _context.PersonPhones.Update(personPhone);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PersonPhone personPhone, CancellationToken ct = default)
    {
        _context.PersonPhones.Remove(personPhone);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string phone, CancellationToken ct = default)
    {
        var normalized = phone.Trim();

        return _context.PersonPhones
            .AsNoTracking()
            .AnyAsync(p => EF.Functions.ILike(p.PhoneCode.CountryCode.Value + p.PhoneNumber.Value, normalized), ct);
    }

    public Task<bool> ExistsPrimaryAsync(int personId, CancellationToken ct = default) =>
        _context.PersonPhones
            .AsNoTracking()
            .AnyAsync(p => p.PersonId == personId && p.IsPrimary, ct);
}
