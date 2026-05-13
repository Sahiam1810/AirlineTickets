using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IInvoiceItemTypeRepository
{
    Task<InvoiceItemType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<InvoiceItemType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<InvoiceItemType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default);
    Task UpdateAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default);
    Task RemoveAsync(InvoiceItemType invoiceItemType, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
