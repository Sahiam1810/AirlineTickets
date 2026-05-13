using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed class DeleteFlightRoleHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlightRole>
{
    public async Task Handle(DeleteFlightRole request, CancellationToken ct)
    {
        var flightRole = await uow.FlightRoles.GetByIdAsync(request.Id, ct);

        if (flightRole is null)
            throw new Exception($"FlightRole with id {request.Id} not found.");

        await uow.FlightRoles.RemoveAsync(flightRole, ct);
        await uow.SaveChangesAsync(ct);
    }
}
