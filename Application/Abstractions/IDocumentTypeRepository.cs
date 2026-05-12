using Domain.Entities.People;

namespace Application.Abstractions;

public interface IDocumentTypeRepository
{
    Task<DocumentType?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DocumentType>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(DocumentType documentType, CancellationToken ct = default);
    Task UpdateAsync(DocumentType documentType, CancellationToken ct = default);
    Task RemoveAsync(DocumentType documentType, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
