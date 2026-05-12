using Domain.Entities.People;

namespace Application.Abstractions;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Client>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Client client, CancellationToken ct = default);
    Task RemoveAsync(Client client, CancellationToken ct = default);
    Task<bool> ExistsByPersonIdAsync(int personId, CancellationToken ct = default);
}
