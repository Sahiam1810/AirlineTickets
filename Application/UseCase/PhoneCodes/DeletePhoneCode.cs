using MediatR;

namespace Application.UseCase.PhoneCodes;

public sealed record DeletePhoneCode(int Id) : IRequest;
