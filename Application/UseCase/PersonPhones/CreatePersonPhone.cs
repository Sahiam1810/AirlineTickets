using MediatR;

namespace Application.UseCase.PersonPhones;

public sealed record CreatePersonPhone(int PersonId, int PhoneCodeId, string PhoneNumber, bool IsPrimary) : IRequest<int>;
