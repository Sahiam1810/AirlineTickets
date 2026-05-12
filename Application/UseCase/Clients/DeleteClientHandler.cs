using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Clients;

public sealed class DeleteClientHandler(IUnitOfWork uow) : IRequestHandler<DeleteClient>
{
    public async Task Handle(DeleteClient request, CancellationToken ct)
    {
        var client = await uow.Clients.GetByIdAsync(request.Id, ct);

        if (client is null)
            throw new Exception($"Client with id {request.Id} not found.");

        await uow.Clients.RemoveAsync(client, ct);
        await uow.SaveChangesAsync(ct);
    }
}
