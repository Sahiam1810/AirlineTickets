using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IPaymentMethodRepository
{
    Task<PaymentMethod?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentMethod>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PaymentMethod>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PaymentMethod paymentMethod, CancellationToken ct = default);
    Task UpdateAsync(PaymentMethod paymentMethod, CancellationToken ct = default);
    Task RemoveAsync(PaymentMethod paymentMethod, CancellationToken ct = default);
    Task<bool> ExistsAsync(string commercialName, CancellationToken ct = default);
}
