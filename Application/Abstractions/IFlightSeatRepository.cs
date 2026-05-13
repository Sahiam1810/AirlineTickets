using Domain.Entities.Flights;
using Domain.ValueObjects.FlightSeats;

namespace Application.Abstractions;

public interface IFlightSeatRepository
{
    Task<FlightSeat?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FlightSeat>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FlightSeat>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(FlightSeat flightSeat, CancellationToken ct = default);
    Task UpdateAsync(FlightSeat flightSeat, CancellationToken ct = default);
    Task RemoveAsync(FlightSeat flightSeat, CancellationToken ct = default);
    Task<bool> ExistsAsync(int flightId, SeatCode seatCode, int? excludedId = null, CancellationToken ct = default);
}
