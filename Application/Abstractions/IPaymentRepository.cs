using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Payments;

namespace Application.Abstractions;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Payment>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Payment payment, CancellationToken ct = default);
    Task UpdateAsync(Payment payment, CancellationToken ct = default);
    Task RemoveAsync(Payment payment, CancellationToken ct = default);
}
