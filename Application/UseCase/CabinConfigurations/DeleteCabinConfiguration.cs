using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed record DeleteCabinConfiguration(int Id) : IRequest;
