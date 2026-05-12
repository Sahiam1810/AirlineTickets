using System;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    IContinent Continents { get; }
    ICountryRepository Countries { get; }
    IRegionRepository Regions { get; }
    ICityRepository Cities { get; }
    IRoadTypeRepository RoadTypes { get; }
    IAddressRepository Addresses { get; }
    IDocumentTypeRepository DocumentTypes { get; }
    IPersonRepository People { get; }
    IEmailDomainRepository EmailDomains { get; }
    IPhoneCodeRepository PhoneCodes { get; }
    IPersonEmailRepository PersonEmails { get; }
    IPersonPhoneRepository PersonPhones { get; }
    IClientRepository Clients { get; }
    IAirlineRepository Airlines { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
}
