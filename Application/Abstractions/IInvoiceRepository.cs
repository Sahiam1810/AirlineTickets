using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Invoice>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Invoice>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Invoice invoice, CancellationToken ct = default);
    Task UpdateAsync(Invoice invoice, CancellationToken ct = default);
    Task RemoveAsync(Invoice invoice, CancellationToken ct = default);
    Task<bool> ExistsAsync(string invoiceNumber, CancellationToken ct = default);
    Task<bool> ExistsByReservationAsync(int reservationId, CancellationToken ct = default);
}
