using Application.Abstractions;
using Domain.Entities.People;
using MediatR;

namespace Application.UseCase.Clients;

public sealed class CreateClientHandler : IRequestHandler<CreateClient, int>
{
    private readonly IUnitOfWork uow;

    public CreateClientHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateClient req, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(req.PersonId, ct);
        if (person is null)
            throw new Exception($"Person with id {req.PersonId} not found.");

        if (await uow.Clients.ExistsByPersonIdAsync(req.PersonId, ct))
            throw new Exception($"Client with person id {req.PersonId} already exists.");

        var client = new Client(req.PersonId);

        await uow.Clients.AddAsync(client, ct);
        await uow.SaveChangesAsync(ct);
        return client.Id;
    }
}
