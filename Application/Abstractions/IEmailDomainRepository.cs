using Domain.Entities.People;

namespace Application.Abstractions;

public interface IEmailDomainRepository
{
    Task<EmailDomain?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<EmailDomain>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<EmailDomain>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(EmailDomain emailDomain, CancellationToken ct = default);
    Task UpdateAsync(EmailDomain emailDomain, CancellationToken ct = default);
    Task RemoveAsync(EmailDomain emailDomain, CancellationToken ct = default);
    Task<bool> ExistsAsync(string domain, CancellationToken ct = default);
}
