using Api.Dtos.Seasons;
using Application.Abstractions;
using Application.UseCase.Seasons;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class SeasonsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public SeasonsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SeasonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SeasonDto>>> GetAll(CancellationToken ct)
    {
        var seasons = await _uow.Seasons.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<SeasonDto>>(seasons);
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

        var seasons = await _uow.Seasons.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Seasons.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<SeasonDto>>(seasons);

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
        var total = await _uow.Seasons.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SeasonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SeasonDto>> GetById(int id, CancellationToken ct)
    {
        var season = await _uow.Seasons.GetByIdAsync(id, ct);
        if (season is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<SeasonDto>(season);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SeasonDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSeasonRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateSeason>(request);
        var id = await _sender.Send(command, ct);
        var season = await _uow.Seasons.GetByIdAsync(id, ct);
        if (season is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<SeasonDto>(season);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSeasonRequest request, CancellationToken ct)
    {
        var existing = await _uow.Seasons.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateSeason>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var season = await _uow.Seasons.GetByIdAsync(id, ct);
        if (season is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteSeason(id), ct);

        return NoContent();
    }
}
