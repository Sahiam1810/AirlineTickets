using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IInvoiceItemRepository
{
    Task<InvoiceItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<InvoiceItem>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<InvoiceItem>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(InvoiceItem invoiceItem, CancellationToken ct = default);
    Task UpdateAsync(InvoiceItem invoiceItem, CancellationToken ct = default);
    Task RemoveAsync(InvoiceItem invoiceItem, CancellationToken ct = default);
    Task<bool> ExistsAsync(int invoiceId, string description, CancellationToken ct = default);
    Task<bool> ExistsByInvoiceAsync(int invoiceId, CancellationToken ct = default);
}
