using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EmailDomains;

public sealed class EmailDomainRepository : IEmailDomainRepository
{
    private readonly AppDbContext _context;

    public EmailDomainRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<EmailDomain?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<EmailDomain>()
            .AsTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<IReadOnlyList<EmailDomain>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<EmailDomain>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<EmailDomain>)t.Result, ct);

    public async Task<IReadOnlyList<EmailDomain>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<EmailDomain> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.EmailDomains.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.EmailDomains
                .Where(e => EF.Functions.ILike(e.Domain.Value, pattern))
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
        IQueryable<EmailDomain> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.EmailDomains.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.EmailDomains
                .Where(e => EF.Functions.ILike(e.Domain.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(EmailDomain emailDomain, CancellationToken ct = default)
    {
        _context.EmailDomains.Add(emailDomain);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(EmailDomain emailDomain, CancellationToken ct = default)
    {
        _context.EmailDomains.Update(emailDomain);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(EmailDomain emailDomain, CancellationToken ct = default)
    {
        _context.EmailDomains.Remove(emailDomain);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string domain, CancellationToken ct = default)
    {
        var pattern = domain.Trim().ToLowerInvariant();

        return _context.EmailDomains
            .AsNoTracking()
            .AnyAsync(e => EF.Functions.ILike(e.Domain.Value, pattern), ct);
    }
}
