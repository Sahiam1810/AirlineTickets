using Application.Abstractions;
using Infrastructure.Context;
using Infrastructure.Repositories.Addresses;
using Infrastructure.Repositories.Aircraft;
using Infrastructure.Repositories.AircraftManufacturers;
using Infrastructure.Repositories.AircraftModels;
using Infrastructure.Repositories.Airlines;
using Infrastructure.Repositories.AirportAirlines;
using Infrastructure.Repositories.Airports;
using Infrastructure.Repositories.AvailabilityStatuses;
using Infrastructure.Repositories.CabinConfigurations;
using Infrastructure.Repositories.CabinTypes;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Clients;
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
using Infrastructure.Repositories.Routes;
using Infrastructure.Repositories.Staff;
using Infrastructure.Repositories.StaffAvailabilities;
using Infrastructure.Repositories.StaffRoles;

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
    public IClientRepository? _client;
    public IAirlineRepository? _airline;
    public IAirportRepository? _airport;
    public IAirportAirlineRepository? _airportAirline;
    public IAircraftManufacturerRepository? _aircraftManufacturer;
    public IAircraftModelRepository? _aircraftModel;
    public IAircraftRepository? _aircraft;
    public ICabinTypeRepository? _cabinType;
    public ICabinConfigurationRepository? _cabinConfiguration;
    public IStaffRoleRepository? _staffRole;
    public IAvailabilityStatusRepository? _availabilityStatus;
    public IStaffAvailabilityRepository? _staffAvailability;
    public IStaffRepository? _staff;
    public IRouteRepository? _route;

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

    public IClientRepository Clients
    {
        get
        {
            if (_client == null)
            {
                _client = new ClientRepository(_contextdb);
            }

            return _client;
        }
    }

    public IAirlineRepository Airlines
    {
        get
        {
            if (_airline == null)
            {
                _airline = new AirlineRepository(_contextdb);
            }

            return _airline;
        }
    }

    public IAirportRepository Airports
    {
        get
        {
            if (_airport == null)
            {
                _airport = new AirportRepository(_contextdb);
            }

            return _airport;
        }
    }

    public IAirportAirlineRepository AirportAirlines
    {
        get
        {
            if (_airportAirline == null)
            {
                _airportAirline = new AirportAirlineRepository(_contextdb);
            }

            return _airportAirline;
        }
    }

    public IAircraftManufacturerRepository AircraftManufacturers
    {
        get
        {
            if (_aircraftManufacturer == null)
            {
                _aircraftManufacturer = new AircraftManufacturerRepository(_contextdb);
            }

            return _aircraftManufacturer;
        }
    }

    public IAircraftModelRepository AircraftModels
    {
        get
        {
            if (_aircraftModel == null)
            {
                _aircraftModel = new AircraftModelRepository(_contextdb);
            }

            return _aircraftModel;
        }
    }

    public IAircraftRepository Aircraft
    {
        get
        {
            if (_aircraft == null)
            {
                _aircraft = new AircraftRepository(_contextdb);
            }

            return _aircraft;
        }
    }

    public ICabinTypeRepository CabinTypes
    {
        get
        {
            if (_cabinType == null)
            {
                _cabinType = new CabinTypeRepository(_contextdb);
            }

            return _cabinType;
        }
    }

    public ICabinConfigurationRepository CabinConfigurations
    {
        get
        {
            if (_cabinConfiguration == null)
            {
                _cabinConfiguration = new CabinConfigurationRepository(_contextdb);
            }

            return _cabinConfiguration;
        }
    }

    public IStaffRoleRepository StaffRoles
    {
        get
        {
            if (_staffRole == null)
            {
                _staffRole = new StaffRoleRepository(_contextdb);
            }

            return _staffRole;
        }
    }

    public IAvailabilityStatusRepository AvailabilityStatuses
    {
        get
        {
            if (_availabilityStatus == null)
            {
                _availabilityStatus = new AvailabilityStatusRepository(_contextdb);
            }

            return _availabilityStatus;
        }
    }

    public IStaffAvailabilityRepository StaffAvailabilities
    {
        get
        {
            if (_staffAvailability == null)
            {
                _staffAvailability = new StaffAvailabilityRepository(_contextdb);
            }

            return _staffAvailability;
        }
    }

    public IStaffRepository Staff
    {
        get
        {
            if (_staff == null)
            {
                _staff = new StaffRepository(_contextdb);
            }

            return _staff;
        }
    }

    public IRouteRepository Routes
    {
        get
        {
            if (_route == null)
            {
                _route = new RouteRepository(_contextdb);
            }

            return _route;
        }
    }
}
