using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed class DeletePhoneCodeHandler(IUnitOfWork uow) : IRequestHandler<DeletePhoneCode>
{
    public async Task Handle(DeletePhoneCode request, CancellationToken ct)
    {
        var phoneCode = await uow.PhoneCodes.GetByIdAsync(request.Id, ct);

        if (phoneCode is null)
            throw new Exception($"PhoneCode with id {request.Id} not found.");

        await uow.PhoneCodes.RemoveAsync(phoneCode, ct);
        await uow.SaveChangesAsync(ct);
    }
}
