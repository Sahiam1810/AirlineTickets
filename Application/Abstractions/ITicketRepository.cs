using Domain.Entities.Tickets;

namespace Application.Abstractions;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Ticket>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Ticket ticket, CancellationToken ct = default);
    Task UpdateAsync(Ticket ticket, CancellationToken ct = default);
    Task RemoveAsync(Ticket ticket, CancellationToken ct = default);
    Task<bool> ExistsAsync(string ticketCode, int? excludedId = null, CancellationToken ct = default);
    Task<bool> ExistsByReservationPassengerAsync(int reservationPassengerId, int? excludedId = null, CancellationToken ct = default);
}
