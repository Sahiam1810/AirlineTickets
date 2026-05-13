using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface ICardIssuerRepository
{
    Task<CardIssuer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CardIssuer>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CardIssuer>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CardIssuer cardIssuer, CancellationToken ct = default);
    Task UpdateAsync(CardIssuer cardIssuer, CancellationToken ct = default);
    Task RemoveAsync(CardIssuer cardIssuer, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
