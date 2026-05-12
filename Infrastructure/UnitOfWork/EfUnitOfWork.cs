using Application.Abstractions;
using Infrastructure.Context;
using Infrastructure.Repositories.Continents;

namespace Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _contextdb;
    public IContinent? _continent;

    public EfUnitOfWork(AppDbContext db)
    {
        _contextdb = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _contextdb.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
    {
        await using var tx = await _contextdb.Database.BeginTransactionAsync(ct);
        try
        {
            await operation(ct);
            await _contextdb.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }

    public IContinent Continents
    {
        get
        {
            if (_continent == null)
            {
                _continent = new ContinentRepository(_contextdb);
            }

            return _continent;
        }
    }
}