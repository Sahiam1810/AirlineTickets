using MediatR;

namespace Application.UseCase.Airlines;

public sealed record DeleteAirline(int Id) : IRequest;
