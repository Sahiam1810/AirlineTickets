using Application.Abstractions;
using MediatR;

namespace Application.UseCase.People;

public sealed class DeletePersonHandler(IUnitOfWork uow) : IRequestHandler<DeletePerson>
{
    public async Task Handle(DeletePerson request, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(request.Id, ct);

        if (person is null)
            throw new Exception($"Person with id {request.Id} not found.");

        await uow.People.RemoveAsync(person, ct);
        await uow.SaveChangesAsync(ct);
    }
}
