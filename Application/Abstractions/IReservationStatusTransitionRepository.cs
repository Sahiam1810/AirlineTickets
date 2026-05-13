using Domain.Entities.Reservations;

namespace Application.Abstractions;

public interface IReservationStatusTransitionRepository
{
    Task<ReservationStatusTransition?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ReservationStatusTransition>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ReservationStatusTransition>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default);
    Task UpdateAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default);
    Task RemoveAsync(ReservationStatusTransition reservationStatusTransition, CancellationToken ct = default);
    Task<bool> ExistsAsync(int fromStatusId, int toStatusId, int? excludedId = null, CancellationToken ct = default);
}
