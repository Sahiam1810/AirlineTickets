using Domain.Entities.Routes;
using Domain.ValueObjects.Seasons;

namespace Application.Abstractions;

public interface ISeasonRepository
{
    Task<Season?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Season>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Season season, CancellationToken ct = default);
    Task UpdateAsync(Season season, CancellationToken ct = default);
    Task RemoveAsync(Season season, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(SeasonName name, CancellationToken ct = default);
}
