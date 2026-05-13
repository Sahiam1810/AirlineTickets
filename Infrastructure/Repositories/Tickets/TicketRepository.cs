using Application.Abstractions;
using Domain.Entities.Tickets;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tickets;

public sealed class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Ticket?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Ticket>())
            .AsTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<Ticket>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Ticket>)t.Result, ct);

    public async Task<IReadOnlyList<Ticket>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<Ticket> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.Tickets)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.Tickets)
                .Where(t =>
                    EF.Functions.ILike(t.TicketCode, pattern) ||
                    EF.Functions.ILike(t.TicketStatus.Name, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.ReservationFlight.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.ReservationFlight.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Ticket> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Tickets.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.Tickets
                .Where(t =>
                    EF.Functions.ILike(t.TicketCode, pattern) ||
                    EF.Functions.ILike(t.TicketStatus.Name, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.ReservationFlight.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.ReservationFlight.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(t.ReservationPassenger.Passenger.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Ticket ticket, CancellationToken ct = default)
    {
        _context.Tickets.Add(ticket);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Ticket ticket, CancellationToken ct = default)
    {
        _context.Tickets.Update(ticket);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Ticket ticket, CancellationToken ct = default)
    {
        _context.Tickets.Remove(ticket);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string ticketCode, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.Tickets
            .AsNoTracking()
            .Where(t => EF.Functions.ILike(t.TicketCode, ticketCode));

        if (excludedId.HasValue)
        {
            query = query.Where(t => t.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    public Task<bool> ExistsByReservationPassengerAsync(int reservationPassengerId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.Tickets
            .AsNoTracking()
            .Where(t => t.ReservationPassengerId == reservationPassengerId);

        if (excludedId.HasValue)
        {
            query = query.Where(t => t.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<Ticket> IncludeDetails(IQueryable<Ticket> query) =>
        query
            .Include(t => t.TicketStatus)
            .Include(t => t.ReservationPassenger)
                .ThenInclude(rp => rp.ReservationFlight)
                    .ThenInclude(rf => rf.Reservation)
            .Include(t => t.ReservationPassenger)
                .ThenInclude(rp => rp.ReservationFlight)
                    .ThenInclude(rf => rf.Flight)
            .Include(t => t.ReservationPassenger)
                .ThenInclude(rp => rp.Passenger)
                    .ThenInclude(p => p.Person);
}
