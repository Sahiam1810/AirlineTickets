using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IPaymentStateRepository
{
    Task<PaymentState?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentState>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PaymentState>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PaymentState paymentState, CancellationToken ct = default);
    Task UpdateAsync(PaymentState paymentState, CancellationToken ct = default);
    Task RemoveAsync(PaymentState paymentState, CancellationToken ct = default);
    Task<bool> ExistsAsync(string name, CancellationToken ct = default);
}
