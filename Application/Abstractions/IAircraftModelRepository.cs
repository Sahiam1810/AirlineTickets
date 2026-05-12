using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;

namespace Application.Abstractions;

public interface IAircraftModelRepository
{
    Task<AircraftModel?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AircraftModel>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AircraftModel>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(AircraftModel aircraftModel, CancellationToken ct = default);
    Task UpdateAsync(AircraftModel aircraftModel, CancellationToken ct = default);
    Task RemoveAsync(AircraftModel aircraftModel, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(int manufacturerId, ModelName modelName, CancellationToken ct = default);
}
