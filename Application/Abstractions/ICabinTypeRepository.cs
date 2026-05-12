using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;

namespace Application.Abstractions;

public interface ICabinTypeRepository
{
    Task<CabinType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CabinType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CabinType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CabinType cabinType, CancellationToken ct = default);
    Task UpdateAsync(CabinType cabinType, CancellationToken ct = default);
    Task RemoveAsync(CabinType cabinType, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(CabinTypeName name, CancellationToken ct = default);
}
