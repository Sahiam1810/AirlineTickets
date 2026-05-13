using Domain.Entities.Reservations;

namespace Application.Abstractions;

public interface IReservationPassengerRepository
{
    Task<ReservationPassenger?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ReservationPassenger>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ReservationPassenger>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default);
    Task UpdateAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default);
    Task RemoveAsync(ReservationPassenger reservationPassenger, CancellationToken ct = default);
    Task<bool> ExistsAsync(int reservationFlightId, int passengerId, int? excludedId = null, CancellationToken ct = default);
}
