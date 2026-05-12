using Application.Abstractions;
using Domain.Entities.People;
using Domain.ValueObjects.PhoneCodes;
using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed class CreatePhoneCodeHandler : IRequestHandler<CreatePhoneCode, int>
{
    private readonly IUnitOfWork uow;

    public CreatePhoneCodeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreatePhoneCode req, CancellationToken ct)
    {
        if (await uow.PhoneCodes.ExistsByCodeAsync(req.CountryCode, ct))
            throw new Exception($"PhoneCode with country code {req.CountryCode} already exists.");

        if (await uow.PhoneCodes.ExistsByNameAsync(req.CountryName, ct))
            throw new Exception($"PhoneCode with country name {req.CountryName} already exists.");

        var countryCode = CountryCode.Create(req.CountryCode);
        var countryName = CountryName.Create(req.CountryName);
        var phoneCode = new PhoneCode(countryCode, countryName);

        await uow.PhoneCodes.AddAsync(phoneCode, ct);
        await uow.SaveChangesAsync(ct);
        return phoneCode.Id;
    }
}
