using Application.Abstractions;
using Domain.Entities.People;
using MediatR;

namespace Application.UseCase.Passengers;

public sealed class CreatePassengerHandler : IRequestHandler<CreatePassenger, int>
{
    private readonly IUnitOfWork uow;

    public CreatePassengerHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePassenger req, CancellationToken ct)
    {
        await ValidateReferences(req.PersonId, req.PassengerTypeId, ct);

        if (await uow.Passengers.ExistsByPersonIdAsync(req.PersonId, null, ct))
            throw new Exception($"Passenger with person id {req.PersonId} already exists.");

        var passenger = new Passenger(req.PersonId, req.PassengerTypeId);

        await uow.Passengers.AddAsync(passenger, ct);
        await uow.SaveChangesAsync(ct);
        return passenger.Id;
    }

    private async Task ValidateReferences(int personId, int passengerTypeId, CancellationToken ct)
    {
        if (await uow.People.GetByIdAsync(personId, ct) is null)
            throw new Exception($"Person with id {personId} not found.");

        if (await uow.PassengerTypes.GetByIdAsync(passengerTypeId, ct) is null)
            throw new Exception($"PassengerType with id {passengerTypeId} not found.");
    }
}
