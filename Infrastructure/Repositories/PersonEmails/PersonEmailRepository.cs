using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PersonEmails;

public sealed class PersonEmailRepository : IPersonEmailRepository
{
    private readonly AppDbContext _context;

    public PersonEmailRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<PersonEmail?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<PersonEmail>()
            .Include(e => e.EmailDomain)
            .AsTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<IReadOnlyList<PersonEmail>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<PersonEmail>()
            .Include(e => e.EmailDomain)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<PersonEmail>)t.Result, ct);

    public async Task<IReadOnlyList<PersonEmail>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<PersonEmail> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PersonEmails
                .Include(e => e.EmailDomain)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PersonEmails
                .Include(e => e.EmailDomain)
                .Where(e =>
                    EF.Functions.ILike(e.EmailUser.Value, pattern) ||
                    EF.Functions.ILike(e.EmailDomain.Domain.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<PersonEmail> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PersonEmails.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PersonEmails
                .Where(e =>
                    EF.Functions.ILike(e.EmailUser.Value, pattern) ||
                    EF.Functions.ILike(e.EmailDomain.Domain.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(PersonEmail personEmail, CancellationToken ct = default)
    {
        _context.PersonEmails.Add(personEmail);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PersonEmail personEmail, CancellationToken ct = default)
    {
        _context.PersonEmails.Update(personEmail);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PersonEmail personEmail, CancellationToken ct = default)
    {
        _context.PersonEmails.Remove(personEmail);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        var parts = normalized.Split('@', 2);

        if (parts.Length != 2)
        {
            return Task.FromResult(false);
        }

        var emailUser = parts[0];
        var domain = parts[1];

        return _context.PersonEmails
            .AsNoTracking()
            .AnyAsync(e =>
                EF.Functions.ILike(e.EmailUser.Value, emailUser) &&
                EF.Functions.ILike(e.EmailDomain.Domain.Value, domain), ct);
    }

    public Task<bool> ExistsPrimaryAsync(int personId, CancellationToken ct = default) =>
        _context.PersonEmails
            .AsNoTracking()
            .AnyAsync(e => e.PersonId == personId && e.IsPrimary, ct);
}
