using Application.Abstractions;
using Domain.Entities.Reservations;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ReservationFlights;

public sealed class ReservationFlightRepository : IReservationFlightRepository
{
    private readonly AppDbContext _context;

    public ReservationFlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ReservationFlight?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationFlight>())
            .AsTracking()
            .FirstOrDefaultAsync(rf => rf.Id == id, ct);

    public Task<IReadOnlyList<ReservationFlight>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<ReservationFlight>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<ReservationFlight>)t.Result, ct);

    public async Task<IReadOnlyList<ReservationFlight>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<ReservationFlight> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.ReservationFlights)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.ReservationFlights)
                .Where(rf =>
                    EF.Functions.ILike(rf.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(rf.Flight.FlightCode.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(rf => rf.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<ReservationFlight> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.ReservationFlights.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.ReservationFlights
                .Where(rf =>
                    EF.Functions.ILike(rf.Reservation.ReservationCode.Value, pattern) ||
                    EF.Functions.ILike(rf.Flight.FlightCode.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(ReservationFlight reservationFlight, CancellationToken ct = default)
    {
        _context.ReservationFlights.Add(reservationFlight);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ReservationFlight reservationFlight, CancellationToken ct = default)
    {
        _context.ReservationFlights.Update(reservationFlight);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ReservationFlight reservationFlight, CancellationToken ct = default)
    {
        _context.ReservationFlights.Remove(reservationFlight);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int reservationId, int flightId, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.ReservationFlights
            .AsNoTracking()
            .Where(rf => rf.ReservationId == reservationId && rf.FlightId == flightId);

        if (excludedId.HasValue)
        {
            query = query.Where(rf => rf.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<ReservationFlight> IncludeDetails(IQueryable<ReservationFlight> query) =>
        query
            .Include(rf => rf.Reservation)
            .Include(rf => rf.Flight);
}
