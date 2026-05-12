using Application.Abstractions;
using Domain.ValueObjects.PassengerTypes;
using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed class UpdatePassengerTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdatePassengerType>
{
    public async Task Handle(UpdatePassengerType request, CancellationToken ct)
    {
        var passengerType = await uow.PassengerTypes.GetByIdAsync(request.Id, ct);

        if (passengerType is null)
            throw new Exception($"PassengerType with id {request.Id} not found.");

        var name = PassengerTypeName.Create(request.Name);
        ValidateAgeRange(request.AgeMin, request.AgeMax);
        var sameName = string.Equals(passengerType.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.PassengerTypes.ExistsByNameAsync(name, ct))
            throw new Exception($"PassengerType with name {name.Value} already exists.");

        passengerType.Update(request.Name, request.AgeMin, request.AgeMax);

        await uow.PassengerTypes.UpdateAsync(passengerType, ct);
        await uow.SaveChangesAsync(ct);
    }

    private static void ValidateAgeRange(int? ageMin, int? ageMax)
    {
        var min = Age.Create(ageMin);
        var max = Age.Create(ageMax);

        if (min is not null && max is not null && min.Value > max.Value)
            throw new Exception("AgeMin cannot be greater than AgeMax.");
    }
}
