using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Addresses;

public sealed class DeleteAddressHandler(IUnitOfWork uow) : IRequestHandler<DeleteAddress>
{
    public async Task Handle(DeleteAddress request, CancellationToken ct)
    {
        var address = await uow.Addresses.GetByIdAsync(request.Id, ct);

        if (address is null)
            throw new Exception($"Address with id {request.Id} not found.");

        await uow.Addresses.RemoveAsync(address, ct);
        await uow.SaveChangesAsync(ct);
    }
}
