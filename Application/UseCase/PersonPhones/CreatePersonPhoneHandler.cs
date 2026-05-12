using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.PersonPhones;
using MediatR;

namespace Application.UseCase.PersonPhones;

public sealed class CreatePersonPhoneHandler : IRequestHandler<CreatePersonPhone, int>
{
    private readonly IUnitOfWork uow;

    public CreatePersonPhoneHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePersonPhone req, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(req.PersonId, ct);
        if (person is null)
            throw new Exception($"Person with id {req.PersonId} not found.");

        var phoneCode = await uow.PhoneCodes.GetByIdAsync(req.PhoneCodeId, ct);
        if (phoneCode is null)
            throw new Exception($"PhoneCode with id {req.PhoneCodeId} not found.");

        var phoneNumber = PhoneNumber.Create(req.PhoneNumber);
        var fullPhone = $"{phoneCode.CountryCode.Value}{phoneNumber.Value}";

        if (await uow.PersonPhones.ExistsAsync(fullPhone, ct))
            throw new Exception($"PersonPhone with phone {fullPhone} already exists.");

        if (req.IsPrimary && await uow.PersonPhones.ExistsPrimaryAsync(req.PersonId, ct))
            throw new Exception($"Person with id {req.PersonId} already has a primary phone.");

        var personPhone = new PersonPhone(req.PersonId, req.PhoneCodeId, phoneNumber, req.IsPrimary);

        await uow.PersonPhones.AddAsync(personPhone, ct);
        await uow.SaveChangesAsync(ct);
        return personPhone.Id;
    }
}
