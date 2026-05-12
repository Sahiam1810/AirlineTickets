using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PhoneCodes;

public sealed class PhoneCodeRepository : IPhoneCodeRepository
{
    private readonly AppDbContext _context;

    public PhoneCodeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<PhoneCode?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<PhoneCode>()
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<IReadOnlyList<PhoneCode>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<PhoneCode>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<PhoneCode>)t.Result, ct);

    public async Task<IReadOnlyList<PhoneCode>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<PhoneCode> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PhoneCodes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PhoneCodes
                .Where(p =>
                    EF.Functions.ILike(p.CountryCode.Value, pattern) ||
                    EF.Functions.ILike(p.CountryName.Value, pattern))
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
        IQueryable<PhoneCode> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.PhoneCodes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.PhoneCodes
                .Where(p =>
                    EF.Functions.ILike(p.CountryCode.Value, pattern) ||
                    EF.Functions.ILike(p.CountryName.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(PhoneCode phoneCode, CancellationToken ct = default)
    {
        _context.PhoneCodes.Add(phoneCode);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PhoneCode phoneCode, CancellationToken ct = default)
    {
        _context.PhoneCodes.Update(phoneCode);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(PhoneCode phoneCode, CancellationToken ct = default)
    {
        _context.PhoneCodes.Remove(phoneCode);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByCodeAsync(string countryCode, CancellationToken ct = default)
    {
        var pattern = countryCode.Trim();

        return _context.PhoneCodes
            .AsNoTracking()
            .AnyAsync(p => EF.Functions.ILike(p.CountryCode.Value, pattern), ct);
    }

    public Task<bool> ExistsByNameAsync(string countryName, CancellationToken ct = default)
    {
        var pattern = countryName.Trim();

        return _context.PhoneCodes
            .AsNoTracking()
            .AnyAsync(p => EF.Functions.ILike(p.CountryName.Value, pattern), ct);
    }
}
