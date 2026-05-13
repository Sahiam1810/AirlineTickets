using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Reservations;

namespace Application.Abstractions;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Reservation?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Reservation>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Reservation reservation, CancellationToken ct = default);
    Task UpdateAsync(Reservation reservation, CancellationToken ct = default);
    Task RemoveAsync(Reservation reservation, CancellationToken ct = default);
    Task<bool> ExistsCodeAsync(string code, CancellationToken ct = default);
}
