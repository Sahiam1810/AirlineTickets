using MediatR;

namespace Application.UseCase.People;

public sealed record CreatePerson(
    int DocumentTypeId,
    string DocumentNumber,
    string FirstName,
    string LastName,
    DateOnly? BirthDate,
    string? Gender,
    int? AddressId) : IRequest<int>;
