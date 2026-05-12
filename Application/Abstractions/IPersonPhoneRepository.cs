using Domain.Entities.People;

namespace Application.Abstractions;

public interface IPersonPhoneRepository
{
    Task<PersonPhone?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PersonPhone>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PersonPhone>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PersonPhone personPhone, CancellationToken ct = default);
    Task UpdateAsync(PersonPhone personPhone, CancellationToken ct = default);
    Task RemoveAsync(PersonPhone personPhone, CancellationToken ct = default);
    Task<bool> ExistsAsync(string phone, CancellationToken ct = default);
    Task<bool> ExistsPrimaryAsync(int personId, CancellationToken ct = default);
}
