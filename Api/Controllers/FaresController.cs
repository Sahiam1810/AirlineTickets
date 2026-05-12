using Api.Dtos.Fares;
using Application.Abstractions;
using Application.UseCase.Fares;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class FaresController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public FaresController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FareDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FareDto>>> GetAll(CancellationToken ct)
    {
        var fares = await _uow.Fares.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<FareDto>>(fares);
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

        var fares = await _uow.Fares.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Fares.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<FareDto>>(fares);

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
        var total = await _uow.Fares.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FareDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FareDto>> GetById(int id, CancellationToken ct)
    {
        var fare = await _uow.Fares.GetByIdAsync(id, ct);
        if (fare is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FareDto>(fare);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FareDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateFareRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateFare>(request);
        var id = await _sender.Send(command, ct);
        var fare = await _uow.Fares.GetByIdAsync(id, ct);
        if (fare is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FareDto>(fare);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFareRequest request, CancellationToken ct)
    {
        var existing = await _uow.Fares.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateFare>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var fare = await _uow.Fares.GetByIdAsync(id, ct);
        if (fare is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteFare(id), ct);

        return NoContent();
    }
}
