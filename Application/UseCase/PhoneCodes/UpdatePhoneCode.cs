using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed record UpdatePhoneCode(int Id, string CountryCode, string CountryName) : IRequest;
