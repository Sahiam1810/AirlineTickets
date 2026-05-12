using Application.Abstractions;
using Domain.ValueObjects.PersonPhones;
using MediatR;

namespace Application.UseCase.PersonPhones;

public sealed class UpdatePersonPhoneHandler(IUnitOfWork uow) : IRequestHandler<UpdatePersonPhone>
{
    public async Task Handle(UpdatePersonPhone request, CancellationToken ct)
    {
        var personPhone = await uow.PersonPhones.GetByIdAsync(request.Id, ct);

        if (personPhone is null)
            throw new Exception($"PersonPhone with id {request.Id} not found.");

        var phoneCode = await uow.PhoneCodes.GetByIdAsync(request.PhoneCodeId, ct);
        if (phoneCode is null)
            throw new Exception($"PhoneCode with id {request.PhoneCodeId} not found.");

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        var fullPhone = $"{phoneCode.CountryCode.Value}{phoneNumber.Value}";
        var currentPhone = $"{personPhone.PhoneCode.CountryCode.Value}{personPhone.PhoneNumber.Value}";
        var samePhone = string.Equals(currentPhone, fullPhone, StringComparison.OrdinalIgnoreCase);

        if (!samePhone && await uow.PersonPhones.ExistsAsync(fullPhone, ct))
            throw new Exception($"PersonPhone with phone {fullPhone} already exists.");

        if (request.IsPrimary && !personPhone.IsPrimary && await uow.PersonPhones.ExistsPrimaryAsync(personPhone.PersonId, ct))
            throw new Exception($"Person with id {personPhone.PersonId} already has a primary phone.");

        personPhone.Update(request.PhoneCodeId, phoneNumber, request.IsPrimary);

        await uow.PersonPhones.UpdateAsync(personPhone, ct);
        await uow.SaveChangesAsync(ct);
    }
}
