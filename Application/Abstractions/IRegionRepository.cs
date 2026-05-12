using Domain.Entities.Geography;

namespace Application.Abstractions;

public interface IRegionRepository
{
    Task<Region?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Region>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Region>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Region region, CancellationToken ct = default);
    Task UpdateAsync(Region region, CancellationToken ct = default);
    Task RemoveAsync(Region region, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, int countryId, CancellationToken ct = default);
}
