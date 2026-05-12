using Domain.Entities.Airlines;

namespace Application.Abstractions;

public interface IAirlineRepository
{
    Task<Airline?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Airline>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Airline>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Airline airline, CancellationToken ct = default);
    Task UpdateAsync(Airline airline, CancellationToken ct = default);
    Task RemoveAsync(Airline airline, CancellationToken ct = default);
    Task<bool> ExistsByIataCodeAsync(string iataCode, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
