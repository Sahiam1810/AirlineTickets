using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightSeats;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.FlightSeats;

public sealed class FlightSeatRepository : IFlightSeatRepository
{
    private readonly AppDbContext _context;

    public FlightSeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FlightSeat?> GetByIdAsync(int id, CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightSeat>())
            .AsTracking()
            .FirstOrDefaultAsync(fs => fs.Id == id, ct);

    public Task<IReadOnlyList<FlightSeat>> GetAllAsync(CancellationToken ct = default) =>
        IncludeDetails(_context.Set<FlightSeat>())
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<FlightSeat>)t.Result, ct);

    public async Task<IReadOnlyList<FlightSeat>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<FlightSeat> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = IncludeDetails(_context.FlightSeats)
                .AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = IncludeDetails(_context.FlightSeats)
                .Where(fs =>
                    EF.Functions.ILike(fs.SeatCode.Value, pattern) ||
                    EF.Functions.ILike(fs.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(fs.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(fs.SeatLocationType.Name.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(fs => fs.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<FlightSeat> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.FlightSeats.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.FlightSeats
                .Where(fs =>
                    EF.Functions.ILike(fs.SeatCode.Value, pattern) ||
                    EF.Functions.ILike(fs.Flight.FlightCode.Value, pattern) ||
                    EF.Functions.ILike(fs.CabinType.Name.Value, pattern) ||
                    EF.Functions.ILike(fs.SeatLocationType.Name.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(FlightSeat flightSeat, CancellationToken ct = default)
    {
        _context.FlightSeats.Add(flightSeat);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FlightSeat flightSeat, CancellationToken ct = default)
    {
        _context.FlightSeats.Update(flightSeat);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FlightSeat flightSeat, CancellationToken ct = default)
    {
        _context.FlightSeats.Remove(flightSeat);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int flightId, SeatCode seatCode, int? excludedId = null, CancellationToken ct = default)
    {
        var query = _context.FlightSeats
            .AsNoTracking()
            .Where(fs => fs.FlightId == flightId && EF.Functions.ILike(fs.SeatCode.Value, seatCode.Value));

        if (excludedId.HasValue)
        {
            query = query.Where(fs => fs.Id != excludedId.Value);
        }

        return query.AnyAsync(ct);
    }

    private static IQueryable<FlightSeat> IncludeDetails(IQueryable<FlightSeat> query) =>
        query
            .Include(fs => fs.Flight)
            .Include(fs => fs.CabinType)
            .Include(fs => fs.SeatLocationType);
}
