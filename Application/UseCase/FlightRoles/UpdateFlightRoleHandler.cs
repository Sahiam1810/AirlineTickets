using Application.Abstractions;
using Domain.ValueObjects.FlightRoles;
using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed class UpdateFlightRoleHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlightRole>
{
    public async Task Handle(UpdateFlightRole request, CancellationToken ct)
    {
        var flightRole = await uow.FlightRoles.GetByIdAsync(request.Id, ct);

        if (flightRole is null)
            throw new Exception($"FlightRole with id {request.Id} not found.");

        var name = FlightRoleName.Create(request.Name);
        var sameName = string.Equals(flightRole.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.FlightRoles.ExistsByNameAsync(name, ct))
            throw new Exception($"FlightRole with name {name.Value} already exists.");

        flightRole.Update(request.Name);

        await uow.FlightRoles.UpdateAsync(flightRole, ct);
        await uow.SaveChangesAsync(ct);
    }
}
