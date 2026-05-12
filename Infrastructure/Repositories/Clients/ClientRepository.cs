using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Clients;

public sealed class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Client?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<Client>()
            .Include(c => c.Person)
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<Client>()
            .Include(c => c.Person)
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Client>)t.Result, ct);

    public async Task<IReadOnlyList<Client>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Client> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Clients
                .Include(c => c.Person)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Clients
                .Include(c => c.Person)
                .Where(c =>
                    EF.Functions.ILike(c.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(c.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(c.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Client> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Clients.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Clients
                .Where(c =>
                    EF.Functions.ILike(c.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(c.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(c.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Client client, CancellationToken ct = default)
    {
        _context.Clients.Add(client);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Client client, CancellationToken ct = default)
    {
        _context.Clients.Remove(client);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByPersonIdAsync(int personId, CancellationToken ct = default) =>
        _context.Clients
            .AsNoTracking()
            .AnyAsync(c => c.PersonId == personId, ct);
}
