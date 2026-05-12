using Api.Dtos.Airlines;
using Application.Abstractions;
using Application.UseCase.Airlines;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AirlinesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AirlinesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AirlineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AirlineDto>>> GetAll(CancellationToken ct)
    {
        var airlines = await _uow.Airlines.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AirlineDto>>(airlines);
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

        var airlines = await _uow.Airlines.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Airlines.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AirlineDto>>(airlines);

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
        var total = await _uow.Airlines.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineDto>> GetById(int id, CancellationToken ct)
    {
        var airline = await _uow.Airlines.GetByIdAsync(id, ct);
        if (airline is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AirlineDto>(airline);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAirlineRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAirline>(request);
        var id = await _sender.Send(command, ct);
        var airline = await _uow.Airlines.GetByIdAsync(id, ct);
        if (airline is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AirlineDto>(airline);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAirlineRequest request, CancellationToken ct)
    {
        var existing = await _uow.Airlines.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAirline>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var airline = await _uow.Airlines.GetByIdAsync(id, ct);
        if (airline is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAirline(id), ct);

        return NoContent();
    }
}
