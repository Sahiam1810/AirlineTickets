using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed record UpdateAircraftManufacturer(int Id, string Name, string Country) : IRequest;
