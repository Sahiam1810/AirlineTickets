using MediatR;

namespace Application.UseCase.Addresses;

public sealed record DeleteAddress(int Id) : IRequest;
