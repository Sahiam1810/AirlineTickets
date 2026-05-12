using Api.Dtos.Countries;
using Application.Abstractions;
using Application.UseCase.Countries;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CountriesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CountriesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CountryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAll(CancellationToken ct)
    {
        var countries = await _uow.Countries.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CountryDto>>(countries);
        return Ok(result);
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var countries = await _uow.Countries.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Countries.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CountryDto>>(countries);

        return Ok(new
        {
            page,
            pageSize,
            total,
            items
        });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.Countries.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CountryDto>> GetById(int id, CancellationToken ct)
    {
        var country = await _uow.Countries.GetByIdAsync(id, ct);
        if (country is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CountryDto>(country);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCountryRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCountry>(request);
        var id = await _sender.Send(command, ct);
        var country = await _uow.Countries.GetByIdAsync(id, ct);
        if (country is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CountryDto>(country);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryRequest request, CancellationToken ct)
    {
        var existing = await _uow.Countries.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCountry>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var country = await _uow.Countries.GetByIdAsync(id, ct);
        if (country is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteCountry(id), ct);

        return NoContent();
    }
}
