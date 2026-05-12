using Domain.Entities.People;
using Domain.ValueObjects.PassengerTypes;

namespace Application.Abstractions;

public interface IPassengerTypeRepository
{
    Task<PassengerType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PassengerType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PassengerType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PassengerType passengerType, CancellationToken ct = default);
    Task UpdateAsync(PassengerType passengerType, CancellationToken ct = default);
    Task RemoveAsync(PassengerType passengerType, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(PassengerTypeName name, CancellationToken ct = default);
}
