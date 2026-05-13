using Domain.Entities.Reservations;

namespace Application.Abstractions;

public interface IReservationFlightRepository
{
    Task<ReservationFlight?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ReservationFlight>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ReservationFlight>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(ReservationFlight reservationFlight, CancellationToken ct = default);
    Task UpdateAsync(ReservationFlight reservationFlight, CancellationToken ct = default);
    Task RemoveAsync(ReservationFlight reservationFlight, CancellationToken ct = default);
    Task<bool> ExistsAsync(int reservationId, int flightId, int? excludedId = null, CancellationToken ct = default);
}
