using Domain.Entities.Tickets;

namespace Application.Abstractions;

public interface ICheckInRepository
{
    Task<CheckIn?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CheckIn>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CheckIn>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(CheckIn checkIn, CancellationToken ct = default);
    Task UpdateAsync(CheckIn checkIn, CancellationToken ct = default);
    Task RemoveAsync(CheckIn checkIn, CancellationToken ct = default);
    Task<bool> ExistsAsync(string boardingPassNumber, int? excludedId = null, CancellationToken ct = default);
    Task<bool> ExistsByTicketAsync(int ticketId, int? excludedId = null, CancellationToken ct = default);
    Task<bool> ExistsByFlightSeatAsync(int flightSeatId, int? excludedId = null, CancellationToken ct = default);
}
