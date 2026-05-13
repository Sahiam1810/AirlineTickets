using Domain.Entities.Flights;
using Domain.ValueObjects.SeatLocationTypes;

namespace Application.Abstractions;

public interface ISeatLocationTypeRepository
{
    Task<SeatLocationType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SeatLocationType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SeatLocationType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(SeatLocationType seatLocationType, CancellationToken ct = default);
    Task UpdateAsync(SeatLocationType seatLocationType, CancellationToken ct = default);
    Task RemoveAsync(SeatLocationType seatLocationType, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(SeatLocationTypeName name, int? excludedId = null, CancellationToken ct = default);
}
