using Domain.Entities.People;

namespace Application.Abstractions;

public interface IPersonEmailRepository
{
    Task<PersonEmail?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PersonEmail>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PersonEmail>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PersonEmail personEmail, CancellationToken ct = default);
    Task UpdateAsync(PersonEmail personEmail, CancellationToken ct = default);
    Task RemoveAsync(PersonEmail personEmail, CancellationToken ct = default);
    Task<bool> ExistsAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsPrimaryAsync(int personId, CancellationToken ct = default);
}
