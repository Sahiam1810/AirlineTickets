using Domain.Entities.Airlines;

namespace Application.Abstractions;

public interface IAirportRepository
{
    Task<Airport?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Airport>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Airport>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Airport airport, CancellationToken ct = default);
    Task UpdateAsync(Airport airport, CancellationToken ct = default);
    Task RemoveAsync(Airport airport, CancellationToken ct = default);
    Task<bool> ExistsByIataCodeAsync(string iataCode, CancellationToken ct = default);
    Task<bool> ExistsByIcaoCodeAsync(string? icaoCode, CancellationToken ct = default);
}
