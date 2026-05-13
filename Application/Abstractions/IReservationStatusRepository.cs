using Domain.Entities.Reservations;
using Domain.ValueObjects.ReservationStatuses;

namespace Application.Abstractions;

public interface IReservationStatusRepository
{
    Task<ReservationStatus?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ReservationStatus>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ReservationStatus>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(ReservationStatus reservationStatus, CancellationToken ct = default);
    Task UpdateAsync(ReservationStatus reservationStatus, CancellationToken ct = default);
    Task RemoveAsync(ReservationStatus reservationStatus, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(ReservationStatusName name, int? excludedId = null, CancellationToken ct = default);
}
