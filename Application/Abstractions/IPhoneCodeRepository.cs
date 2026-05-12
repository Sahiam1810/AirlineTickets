using Domain.Entities.People;

namespace Application.Abstractions;

public interface IPhoneCodeRepository
{
    Task<PhoneCode?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PhoneCode>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PhoneCode>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(PhoneCode phoneCode, CancellationToken ct = default);
    Task UpdateAsync(PhoneCode phoneCode, CancellationToken ct = default);
    Task RemoveAsync(PhoneCode phoneCode, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string countryCode, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string countryName, CancellationToken ct = default);
}
