using Domain.Entities.Routes;

namespace Application.Abstractions;

public interface IFareRepository
{
    Task<Fare?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Fare>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Fare>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Fare fare, CancellationToken ct = default);
    Task UpdateAsync(Fare fare, CancellationToken ct = default);
    Task RemoveAsync(Fare fare, CancellationToken ct = default);
    Task<bool> ExistsAsync(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        DateOnly? validFrom,
        DateOnly? validTo,
        int? excludedId = null,
        CancellationToken ct = default);
}
