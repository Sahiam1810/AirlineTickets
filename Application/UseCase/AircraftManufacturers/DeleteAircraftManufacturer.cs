using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed record DeleteAircraftManufacturer(int Id) : IRequest;
