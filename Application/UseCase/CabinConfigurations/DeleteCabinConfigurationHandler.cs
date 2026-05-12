using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed class DeleteCabinConfigurationHandler(IUnitOfWork uow) : IRequestHandler<DeleteCabinConfiguration>
{
    public async Task Handle(DeleteCabinConfiguration request, CancellationToken ct)
    {
        var cabinConfiguration = await uow.CabinConfigurations.GetByIdAsync(request.Id, ct);

        if (cabinConfiguration is null)
            throw new Exception($"CabinConfiguration with id {request.Id} not found.");

        await uow.CabinConfigurations.RemoveAsync(cabinConfiguration, ct);
        await uow.SaveChangesAsync(ct);
    }
}
