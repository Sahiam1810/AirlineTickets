using System;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    IContinent Continents { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
}
