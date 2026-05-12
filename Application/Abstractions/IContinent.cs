using System;
using Domain.Entities.Geography;

namespace Application.Abstractions;

public interface IContinent
{
    Task<Continent?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Continent?> GetByNameAsync(string name, CancellationToken ct = default);
    
    Task<IReadOnlyList<Continent>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Continent>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Continent continent, CancellationToken ct = default);
    Task UpdateAsync(Continent continent, CancellationToken ct = default);
    Task RemoveAsync(Continent continent, CancellationToken ct = default);
    Task<bool> ExistsNamedAsync(string name, CancellationToken ct = default);
}