using Domain.Entities.People;

namespace Application.Abstractions;

public interface IPassengerRepository
{
    Task<Passenger?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Passenger?> GetByPersonIdAsync(int personId, CancellationToken ct = default);
    Task<IReadOnlyList<Passenger>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Passenger>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Passenger passenger, CancellationToken ct = default);
    Task UpdateAsync(Passenger passenger, CancellationToken ct = default);
    Task RemoveAsync(Passenger passenger, CancellationToken ct = default);
    Task<bool> ExistsByPersonIdAsync(int personId, int? excludedId = null, CancellationToken ct = default);
}
