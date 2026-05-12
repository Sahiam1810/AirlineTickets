using MediatR;

namespace Application.UseCase.Addresses;

public sealed record UpdateAddress(
    int Id,
    int RoadTypeId,
    string StreetName,
    string? Number,
    string? Complement,
    int CityId,
    string? PostalCode) : IRequest;
