using Application.Abstractions;
using Infrastructure.Context;
using Infrastructure.Repositories.Cities;
using Infrastructure.Repositories.Continents;
using Infrastructure.Repositories.Countries;
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
}
