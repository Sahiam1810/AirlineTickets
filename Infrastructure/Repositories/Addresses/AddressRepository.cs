using Application.Abstractions;
using Domain.Entities.Location;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Addresses;

public sealed class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Address?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Address>()
            .Include(a => a.RoadType)
            .Include(a => a.City)
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<IReadOnlyList<Address>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Address>()
            .Include(a => a.RoadType)
            .Include(a => a.City)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Address>)t.Result, ct);

    public async Task<IReadOnlyList<Address>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Address> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Addresses
                .Include(a => a.RoadType)
                .Include(a => a.City)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Addresses
                .Include(a => a.RoadType)
                .Include(a => a.City)
                .Where(a =>
                    EF.Functions.ILike(a.StreetName.Value, pattern) ||
                    (a.Number != null && EF.Functions.ILike(a.Number.Value, pattern)) ||
                    (a.Complement != null && EF.Functions.ILike(a.Complement.Value, pattern)) ||
                    (a.PostalCode != null && EF.Functions.ILike(a.PostalCode.Value, pattern)))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Address> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Addresses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Addresses
                .Where(a =>
                    EF.Functions.ILike(a.StreetName.Value, pattern) ||
                    (a.Number != null && EF.Functions.ILike(a.Number.Value, pattern)) ||
                    (a.Complement != null && EF.Functions.ILike(a.Complement.Value, pattern)) ||
                    (a.PostalCode != null && EF.Functions.ILike(a.PostalCode.Value, pattern)))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Address address, CancellationToken ct = default)
    {
        _context.Addresses.Add(address);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Address address, CancellationToken ct = default)
    {
        _context.Addresses.Update(address);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Address address, CancellationToken ct = default)
    {
        _context.Addresses.Remove(address);
        return Task.CompletedTask;
    }
}
