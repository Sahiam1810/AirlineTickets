using Domain.Entities.Tickets;

namespace Application.Abstractions;

public interface ITicketStatusRepository
{
    Task<TicketStatus?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<TicketStatus>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TicketStatus>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(TicketStatus ticketStatus, CancellationToken ct = default);
    Task UpdateAsync(TicketStatus ticketStatus, CancellationToken ct = default);
    Task RemoveAsync(TicketStatus ticketStatus, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, int? excludedId = null, CancellationToken ct = default);
}
