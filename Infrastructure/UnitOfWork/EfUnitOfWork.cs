using Application.Abstractions;
using Infrastructure.Context;
using Infrastructure.Repositories.Addresses;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Continents;
using Infrastructure.Repositories.Countries;
using Infrastructure.Repositories.DocumentTypes;
using Infrastructure.Repositories.EmailDomains;
using Infrastructure.Repositories.People;
using Infrastructure.Repositories.PersonEmails;
using Infrastructure.Repositories.PersonPhones;
using Infrastructure.Repositories.PhoneCodes;
using Infrastructure.Repositories.Regions;
using Infrastructure.Repositories.RoadTypes;

namespace Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _contextdb;
    public IContinent? _continent;
    public ICountryRepository? _country;
    public IRegionRepository? _region;
    public ICityRepository? _city;
    public IRoadTypeRepository? _roadType;
    public IAddressRepository? _address;
    public IDocumentTypeRepository? _documentType;
    public IPersonRepository? _person;
    public IEmailDomainRepository? _emailDomain;
    public IPhoneCodeRepository? _phoneCode;
    public IPersonEmailRepository? _personEmail;
    public IPersonPhoneRepository? _personPhone;

    public EfUnitOfWork(AppDbContext db)
    {
        _contextdb = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _contextdb.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
    {
        await using var tx = await _contextdb.Database.BeginTransactionAsync(ct);
        try
        {
            await operation(ct);
            await _contextdb.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }

    public IContinent Continents
    {
        get
        {
            if (_continent == null)
            {
                _continent = new ContinentRepository(_contextdb);
            }

            return _continent;
        }
    }

    public ICountryRepository Countries
    {
        get
        {
            if (_country == null)
            {
                _country = new CountryRepository(_contextdb);
            }

            return _country;
        }
    }

    public IRegionRepository Regions
    {
        get
        {
            if (_region == null)
            {
                _region = new RegionRepository(_contextdb);
            }

            return _region;
        }
    }

    public ICityRepository Cities
    {
        get
        {
            if (_city == null)
            {
                _city = new CityRepository(_contextdb);
            }

            return _city;
        }
    }

    public IRoadTypeRepository RoadTypes
    {
        get
        {
            if (_roadType == null)
            {
                _roadType = new RoadTypeRepository(_contextdb);
            }

            return _roadType;
        }
    }

    public IAddressRepository Addresses
    {
        get
        {
            if (_address == null)
            {
                _address = new AddressRepository(_contextdb);
            }

            return _address;
        }
    }

    public IDocumentTypeRepository DocumentTypes
    {
        get
        {
            if (_documentType == null)
            {
                _documentType = new DocumentTypeRepository(_contextdb);
            }

            return _documentType;
        }
    }

    public IPersonRepository People
    {
        get
        {
            if (_person == null)
            {
                _person = new PersonRepository(_contextdb);
            }

            return _person;
        }
    }

    public IEmailDomainRepository EmailDomains
    {
        get
        {
            if (_emailDomain == null)
            {
                _emailDomain = new EmailDomainRepository(_contextdb);
            }

            return _emailDomain;
        }
    }

    public IPhoneCodeRepository PhoneCodes
    {
        get
        {
            if (_phoneCode == null)
            {
                _phoneCode = new PhoneCodeRepository(_contextdb);
            }

            return _phoneCode;
        }
    }

    public IPersonEmailRepository PersonEmails
    {
        get
        {
            if (_personEmail == null)
            {
                _personEmail = new PersonEmailRepository(_contextdb);
            }

            return _personEmail;
        }
    }

    public IPersonPhoneRepository PersonPhones
    {
        get
        {
            if (_personPhone == null)
            {
                _personPhone = new PersonPhoneRepository(_contextdb);
            }

            return _personPhone;
        }
    }
}
