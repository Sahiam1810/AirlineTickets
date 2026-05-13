using Domain.Entities.Auth;

namespace Application.Abstractions;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Session>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Session>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Session session, CancellationToken ct = default);
    Task UpdateAsync(Session session, CancellationToken ct = default);
    Task RemoveAsync(Session session, CancellationToken ct = default);
    Task<Session?> GetActiveByUserIdAsync(int userId, CancellationToken ct = default);
}
