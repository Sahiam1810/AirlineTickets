using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Person?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Person>()
            .Include(p => p.DocumentType)
            .Include(p => p.Address)
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Person>()
            .Include(p => p.DocumentType)
            .Include(p => p.Address)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Person>)t.Result, ct);

    public async Task<IReadOnlyList<Person>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Person> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.People
                .Include(p => p.DocumentType)
                .Include(p => p.Address)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.People
                .Include(p => p.DocumentType)
                .Include(p => p.Address)
                .Where(p =>
                    EF.Functions.ILike(p.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(p.FirstName.Value, pattern) ||
                    EF.Functions.ILike(p.LastName.Value, pattern) ||
                    (p.Gender != null && EF.Functions.ILike(p.Gender.Value, pattern)))
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
        IQueryable<Person> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.People.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.People
                .Where(p =>
                    EF.Functions.ILike(p.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(p.FirstName.Value, pattern) ||
                    EF.Functions.ILike(p.LastName.Value, pattern) ||
                    (p.Gender != null && EF.Functions.ILike(p.Gender.Value, pattern)))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Person person, CancellationToken ct = default)
    {
        _context.People.Add(person);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Person person, CancellationToken ct = default)
    {
        _context.People.Update(person);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Person person, CancellationToken ct = default)
    {
        _context.People.Remove(person);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int documentTypeId, string documentNumber, CancellationToken ct = default)
    {
        var pattern = documentNumber.Trim();

        return _context.People
            .AsNoTracking()
            .AnyAsync(p =>
                p.DocumentTypeId == documentTypeId &&
                EF.Functions.ILike(p.DocumentNumber.Value, pattern), ct);
    }
}
