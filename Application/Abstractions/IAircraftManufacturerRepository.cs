using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;

namespace Application.Abstractions;

public interface IAircraftManufacturerRepository
{
    Task<AircraftManufacturer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AircraftManufacturer>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AircraftManufacturer>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default);
    Task UpdateAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default);
    Task RemoveAsync(AircraftManufacturer aircraftManufacturer, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(ManufacturerName name, CancellationToken ct = default);
}
