using Application.Abstractions;
using Domain.Entities.People;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.DocumentTypes;

public sealed class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly AppDbContext _context;

    public DocumentTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<DocumentType?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Set<DocumentType>()
            .AsTracking()
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<IReadOnlyList<DocumentType>> GetAllAsync(CancellationToken ct = default) =>
        _context.Set<DocumentType>()
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<DocumentType>)t.Result, ct);

    public async Task<IReadOnlyList<DocumentType>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        IQueryable<DocumentType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.DocumentTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.DocumentTypes
                .Where(d =>
                    EF.Functions.ILike(d.Name.Value, pattern) ||
                    EF.Functions.ILike(d.Code.Value, pattern))
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<DocumentType> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.DocumentTypes.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim()}%";

            query = _context.DocumentTypes
                .Where(d =>
                    EF.Functions.ILike(d.Name.Value, pattern) ||
                    EF.Functions.ILike(d.Code.Value, pattern))
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(DocumentType documentType, CancellationToken ct = default)
    {
        _context.DocumentTypes.Add(documentType);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(DocumentType documentType, CancellationToken ct = default)
    {
        _context.DocumentTypes.Update(documentType);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(DocumentType documentType, CancellationToken ct = default)
    {
        _context.DocumentTypes.Remove(documentType);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
    {
        var pattern = code.Trim();

        return _context.DocumentTypes
            .AsNoTracking()
            .AnyAsync(d => EF.Functions.ILike(d.Code.Value, pattern), ct);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        var pattern = name.Trim();

        return _context.DocumentTypes
            .AsNoTracking()
            .AnyAsync(d => EF.Functions.ILike(d.Name.Value, pattern), ct);
    }
}
