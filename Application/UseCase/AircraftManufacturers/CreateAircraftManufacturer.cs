using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed record CreateAircraftManufacturer(string Name, string Country) : IRequest<int>;
