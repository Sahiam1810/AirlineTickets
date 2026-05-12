using MediatR;

namespace Application.UseCase.AircraftModels;

public sealed record DeleteAircraftModel(int Id) : IRequest;
