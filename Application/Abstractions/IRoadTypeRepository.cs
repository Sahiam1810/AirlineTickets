using Domain.Entities.Location;

namespace Application.Abstractions;

public interface IRoadTypeRepository
{
    Task<RoadType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<RoadType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RoadType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(RoadType roadType, CancellationToken ct = default);
    Task UpdateAsync(RoadType roadType, CancellationToken ct = default);
    Task RemoveAsync(RoadType roadType, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
