using Api.Dtos.AircraftModels;
using Application.Abstractions;
using Application.UseCase.AircraftModels;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AircraftModelsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AircraftModelsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AircraftModelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AircraftModelDto>>> GetAll(CancellationToken ct)
    {
        var aircraftModels = await _uow.AircraftModels.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AircraftModelDto>>(aircraftModels);
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

        var aircraftModels = await _uow.AircraftModels.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.AircraftModels.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AircraftModelDto>>(aircraftModels);

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
        var total = await _uow.AircraftModels.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AircraftModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AircraftModelDto>> GetById(int id, CancellationToken ct)
    {
        var aircraftModel = await _uow.AircraftModels.GetByIdAsync(id, ct);
        if (aircraftModel is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftModelDto>(aircraftModel);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AircraftModelDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAircraftModelRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAircraftModel>(request);
        var id = await _sender.Send(command, ct);
        var aircraftModel = await _uow.AircraftModels.GetByIdAsync(id, ct);
        if (aircraftModel is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftModelDto>(aircraftModel);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAircraftModelRequest request, CancellationToken ct)
    {
        var existing = await _uow.AircraftModels.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAircraftModel>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var aircraftModel = await _uow.AircraftModels.GetByIdAsync(id, ct);
        if (aircraftModel is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAircraftModel(id), ct);

        return NoContent();
    }
}
