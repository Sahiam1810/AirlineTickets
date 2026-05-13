using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IPaymentMethodTypeRepository
{
    Task<PaymentMethodType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentMethodType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PaymentMethodType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default);
    Task UpdateAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default);
    Task RemoveAsync(PaymentMethodType paymentMethodType, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
