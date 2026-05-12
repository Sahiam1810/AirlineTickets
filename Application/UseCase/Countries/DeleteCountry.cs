using MediatR;

namespace Application.UseCase.Countries;

public sealed record DeleteCountry(int Id) : IRequest;
