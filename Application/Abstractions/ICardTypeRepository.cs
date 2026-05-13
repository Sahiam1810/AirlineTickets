using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface ICardTypeRepository
{
    Task<CardType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CardType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CardType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CardType cardType, CancellationToken ct = default);
    Task UpdateAsync(CardType cardType, CancellationToken ct = default);
    Task RemoveAsync(CardType cardType, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
