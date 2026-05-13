using Application.Abstractions;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Users
            .AsTracking()
            .Include(u => u.Person)
            .Include(u => u.SystemRole)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default) =>
        _context.Users
            .AsNoTracking()
            .Include(u => u.Person)
            .Include(u => u.SystemRole)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<User>)t.Result, ct);

    public async Task<IReadOnlyList<User>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        IQueryable<User> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Users
                .Include(u => u.Person)
                .Include(u => u.SystemRole)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Users
                .Include(u => u.Person)
                .Include(u => u.SystemRole)
                .Where(u => EF.Functions.ILike(u.Username.Value, pattern)
                    || EF.Functions.ILike(u.SystemRole.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<User> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Users.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Users
                .Where(u => EF.Functions.ILike(u.Username.Value, pattern)
                    || EF.Functions.ILike(u.SystemRole.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default)
    {
        var trimmed = username.Trim();

        return _context.Users
            .AsNoTracking()
            .AnyAsync(u => EF.Functions.ILike(u.Username.Value, trimmed), ct);
    }
}
