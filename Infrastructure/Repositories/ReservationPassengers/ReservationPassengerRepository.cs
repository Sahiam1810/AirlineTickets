using Application.Abstractions;
using Domain.Entities.Reservations;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ReservationPassengers;

public sealed class ReservationPassengerRepository : IReservationPassengerRepository
{
    private readonly AppDbContext _context;

    public ReservationPassengerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ReservationPassenger?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationPassenger>())
            .AsTracking()
            .FirstOrDefaultAsync(rp => rp.Id == id, ct);

    public Task<IReadOnlyList<ReservationPassenger>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationPassenger>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<ReservationPassenger>)t.Result, ct);

    public async Task<IReadOnlyList<ReservationPassenger>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<ReservationPassenger> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.ReservationPassengers)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.ReservationPassengers)
                .Where(rp =>
                    EF.Functions.ILike(rp.ReservationFlight.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(rp.ReservationFlight.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rp => rp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<ReservationPassenger> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.ReservationPassengers.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.ReservationPassengers
                .Where(rp =>
                    EF.Functions.ILike(rp.ReservationFlight.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(rp.ReservationFlight.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.DocumentNumber.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.FirstName.Value, pattern) ||
                    EF.Functions.ILike(rp.Passenger.Person.LastName.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default)
    {
        _context.ReservationPassengers.Add(reservationPassenger);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default)
    {
        _context.ReservationPassengers.Update(reservationPassenger);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default)
    {
        _context.ReservationPassengers.Remove(reservationPassenger);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int reservationFlightId, int passengerId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.ReservationPassengers
            .AsNoTracking()
            .Where(rp => rp.ReservationFlightId == reservationFlightId && rp.PassengerId == passengerId);

        if (excludedId.HasValue)
        {
            query = query.Where(rp => rp.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<ReservationPassenger> IncludeDetails(IQueryable<ReservationPassenger> query) =>
        query
            .Include(rp => rp.ReservationFlight)
                .ThenInclude(rf => rf.Reservation)
            .Include(rp => rp.ReservationFlight)
                .ThenInclude(rf => rf.Flight)
            .Include(rp => rp.Passenger)
                .ThenInclude(p => p.Person);
}
