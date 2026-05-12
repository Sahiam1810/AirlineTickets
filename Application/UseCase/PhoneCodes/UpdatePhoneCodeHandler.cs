using Application.Abstractions;
using Domain.ValueObjects.PhoneCodes;
using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed class UpdatePhoneCodeHandler(IUnitOfWork uow) : IRequestHandler<UpdatePhoneCode>
{
    public async Task Handle(UpdatePhoneCode request, CancellationToken ct)
    {
        var phoneCode = await uow.PhoneCodes.GetByIdAsync(request.Id, ct);

        if (phoneCode is null)
            throw new Exception($"PhoneCode with id {request.Id} not found.");

        var countryCode = CountryCode.Create(request.CountryCode);
        var countryName = CountryName.Create(request.CountryName);

        var sameCode = string.Equals(phoneCode.CountryCode.Value, countryCode.Value, StringComparison.OrdinalIgnoreCase);
        var sameName = string.Equals(phoneCode.CountryName.Value, countryName.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameCode && await uow.PhoneCodes.ExistsByCodeAsync(countryCode.Value, ct))
            throw new Exception($"PhoneCode with country code {request.CountryCode} already exists.");

        if (!sameName && await uow.PhoneCodes.ExistsByNameAsync(countryName.Value, ct))
            throw new Exception($"PhoneCode with country name {request.CountryName} already exists.");

        phoneCode.Update(countryCode, countryName);

        await uow.PhoneCodes.UpdateAsync(phoneCode, ct);
        await uow.SaveChangesAsync(ct);
    }
}
