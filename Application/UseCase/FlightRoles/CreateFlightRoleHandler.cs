using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightRoles;
using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed class CreateFlightRoleHandler : IRequestHandler<CreateFlightRole, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightRoleHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlightRole req, CancellationToken ct)
    {
        var name = FlightRoleName.Create(req.Name);

        if (await uow.FlightRoles.ExistsByNameAsync(name, ct))
            throw new Exception($"FlightRole with name {name.Value} already exists.");

        var flightRole = new FlightRole(req.Name);

        await uow.FlightRoles.AddAsync(flightRole, ct);
        await uow.SaveChangesAsync(ct);
        return flightRole.Id;
    }
}
