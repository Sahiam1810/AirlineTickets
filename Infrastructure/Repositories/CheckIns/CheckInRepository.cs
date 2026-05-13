using Application.Abstractions;
using Domain.Entities.Tickets;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CheckIns;

public sealed class CheckInRepository : ICheckInRepository
{
    private readonly AppDbContext _context;

    public CheckInRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CheckIn?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<CheckIn>())
            .AsTracking()
            .FirstOrDefaultAsync(ci => ci.Id == id, ct);

    public Task<IReadOnlyList<CheckIn>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<CheckIn>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<CheckIn>)t.Result, ct);

    public async Task<IReadOnlyList<CheckIn>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<CheckIn> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.CheckIns)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.CheckIns)
                .Where(ci =>
                    EF.Functions.ILike(ci.BoardingPassNumber, pattern) ||
                    EF.Functions.ILike(ci.Ticket.TicketCode, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(ci.FlightSeat.SeatCode.Value, pattern) ||
                    EF.Functions.ILike(ci.CheckInStatus.Name, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(ci => ci.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<CheckIn> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.CheckIns.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.CheckIns
                .Where(ci =>
                    EF.Functions.ILike(ci.BoardingPassNumber, pattern) ||
                    EF.Functions.ILike(ci.Ticket.TicketCode, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(ci.Staff.Person.LastName.Value, pattern) ||
                    EF.Functions.ILike(ci.FlightSeat.SeatCode.Value, pattern) ||
                    EF.Functions.ILike(ci.CheckInStatus.Name, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(CheckIn checkIn, CancellationToken ct = default)
    {
        _context.CheckIns.Add(checkIn);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CheckIn checkIn, CancellationToken ct = default)
    {
        _context.CheckIns.Update(checkIn);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CheckIn checkIn, CancellationToken ct = default)
    {
        _context.CheckIns.Remove(checkIn);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string boardingPassNumber, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.CheckIns
            .AsNoTracking()
            .Where(ci => EF.Functions.ILike(ci.BoardingPassNumber, boardingPassNumber));

        if (excludedId.HasValue)
        {
            query = query.Where(ci => ci.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    public Task<bool> ExistsByTicketAsync(int ticketId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.CheckIns
            .AsNoTracking()
            .Where(ci => ci.TicketId == ticketId);

        if (excludedId.HasValue)
        {
            query = query.Where(ci => ci.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    public Task<bool> ExistsByFlightSeatAsync(int flightSeatId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.CheckIns
            .AsNoTracking()
            .Where(ci => ci.FlightSeatId == flightSeatId);

        if (excludedId.HasValue)
        {
            query = query.Where(ci => ci.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<CheckIn> IncludeDetails(IQueryable<CheckIn> query) =>
        query
            .Include(ci => ci.Ticket)
            .Include(ci => ci.Staff)
                .ThenInclude(s => s.Person)
            .Include(ci => ci.FlightSeat)
            .Include(ci => ci.CheckInStatus);
}
