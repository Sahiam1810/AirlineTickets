using Application.Abstractions;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Session?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Sessions
            .AsTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<IReadOnlyList<Session>> GetAllAsync(CancellationToken ct = default) =>
        _context.Sessions
            .AsNoTracking()
            .Include(s => s.User)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Session>)t.Result, ct);

    public async Task<IReadOnlyList<Session>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        IQueryable<Session> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Sessions
                .Include(s => s.User)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Sessions
                .Include(s => s.User)
                .Where(s => (s.IpAddress != null && EF.Functions.ILike(s.IpAddress.Value, pattern))
                    || EF.Functions.ILike(s.User.Username.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(s => s.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Session> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Sessions.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Sessions
                .Include(s => s.User)
                .Where(s => (s.IpAddress != null && EF.Functions.ILike(s.IpAddress.Value, pattern))
                    || EF.Functions.ILike(s.User.Username.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Session session, CancellationToken ct = default)
    {
        _context.Sessions.Add(session);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Session session, CancellationToken ct = default)
    {
        _context.Sessions.Update(session);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Session session, CancellationToken ct = default)
    {
        _context.Sessions.Remove(session);
        return Task.CompletedTask;
    }

    public Task<Session?> GetActiveByUserIdAsync(int userId, CancellationToken ct = default) =>
        _context.Sessions
            .AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive, ct);
}
