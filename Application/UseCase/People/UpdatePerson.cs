using MediatR;

namespace Application.UseCase.People;

public sealed record UpdatePerson(
    int Id,
    int DocumentTypeId,
    string DocumentNumber,
    string FirstName,
    string LastName,
    DateOnly? BirthDate,
    string? Gender,
    int? AddressId) : IRequest;
