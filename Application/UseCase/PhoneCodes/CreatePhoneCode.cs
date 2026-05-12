using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed record CreatePhoneCode(string CountryCode, string CountryName) : IRequest<int>;
