using Domain.Entities.Aircraft;

namespace Application.Abstractions;

public interface ICabinConfigurationRepository
{
    Task<CabinConfiguration?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CabinConfiguration>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CabinConfiguration>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default);
    Task UpdateAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default);
    Task RemoveAsync(CabinConfiguration cabinConfiguration, CancellationToken ct = default);
    Task<bool> ExistsAsync(int aircraftId, int cabinTypeId, int? excludedId = null, CancellationToken ct = default);
}
