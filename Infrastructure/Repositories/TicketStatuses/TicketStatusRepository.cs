using Application.Abstractions;
using Domain.Entities.Tickets;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TicketStatuses;

public sealed class TicketStatusRepository : ITicketStatusRepository
{
    private readonly AppDbContext _context;

    public TicketStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<TicketStatus?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<TicketStatus>()
            .AsTracking()
            .FirstOrDefaultAsync(ts => ts.Id == id, ct);

    public Task<IReadOnlyList<TicketStatus>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<TicketStatus>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<TicketStatus>)t.Result, ct);

    public async Task<IReadOnlyList<TicketStatus>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<TicketStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.TicketStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.TicketStatuses
                .Where(ts => EF.Functions.ILike(ts.Name, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(ts => ts.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<TicketStatus> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.TicketStatuses.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.TicketStatuses
                .Where(ts => EF.Functions.ILike(ts.Name, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(TicketStatus ticketStatus, CancellationToken ct = default)
    {
        _context.TicketStatuses.Add(ticketStatus);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TicketStatus ticketStatus, CancellationToken ct = default)
    {
        _context.TicketStatuses.Update(ticketStatus);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TicketStatus ticketStatus, CancellationToken ct = default)
    {
        _context.TicketStatuses.Remove(ticketStatus);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.TicketStatuses
            .AsNoTracking()
            .Where(ts => EF.Functions.ILike(ts.Name, name));

        if (excludedId.HasValue)
        {
            query = query.Where(ts => ts.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }
}
