using MediatR;

namespace Application.UseCase.Cities;

public sealed record DeleteCity(int Id) : IRequest;
