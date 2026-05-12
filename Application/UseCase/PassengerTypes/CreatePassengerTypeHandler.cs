using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.PassengerTypes;
using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed class CreatePassengerTypeHandler : IRequestHandler<CreatePassengerType, int>
{
    private readonly IUnitOfWork uow;

    public CreatePassengerTypeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePassengerType req, CancellationToken ct)
    {
        var name = PassengerTypeName.Create(req.Name);
        ValidateAgeRange(req.AgeMin, req.AgeMax);

        if (await uow.PassengerTypes.ExistsByNameAsync(name, ct))
            throw new Exception($"PassengerType with name {name.Value} already exists.");

        var passengerType = new PassengerType(req.Name, req.AgeMin, req.AgeMax);

        await uow.PassengerTypes.AddAsync(passengerType, ct);
        await uow.SaveChangesAsync(ct);
        return passengerType.Id;
    }

    private static void ValidateAgeRange(int? ageMin, int? ageMax)
    {
        var min = Age.Create(ageMin);
        var max = Age.Create(ageMax);

        if (min is not null && max is not null && min.Value > max.Value)
            throw new Exception("AgeMin cannot be greater than AgeMax.");
    }
}
