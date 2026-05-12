using Domain.Entities.Location;

namespace Application.Abstractions;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Address>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Address>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Address address, CancellationToken ct = default);
    Task UpdateAsync(Address address, CancellationToken ct = default);
    Task RemoveAsync(Address address, CancellationToken ct = default);
}
