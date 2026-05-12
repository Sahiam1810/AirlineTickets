using MediatR;

namespace Application.UseCase.Addresses;

public sealed record CreateAddress(
    int RoadTypeId,
    string StreetName,
    string? Number,
    string? Complement,
    int CityId,
    string? PostalCode) : IRequest<int>;
