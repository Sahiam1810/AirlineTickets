using Api.Dtos.CabinConfigurations;
using Application.Abstractions;
using Application.UseCase.CabinConfigurations;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CabinConfigurationsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CabinConfigurationsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CabinConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CabinConfigurationDto>>> GetAll(CancellationToken ct)
    {
        var cabinConfigurations = await _uow.CabinConfigurations.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CabinConfigurationDto>>(cabinConfigurations);
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

        var cabinConfigurations = await _uow.CabinConfigurations.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.CabinConfigurations.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CabinConfigurationDto>>(cabinConfigurations);

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
        var total = await _uow.CabinConfigurations.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CabinConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CabinConfigurationDto>> GetById(int id, CancellationToken ct)
    {
        var cabinConfiguration = await _uow.CabinConfigurations.GetByIdAsync(id, ct);
        if (cabinConfiguration is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CabinConfigurationDto>(cabinConfiguration);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CabinConfigurationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCabinConfigurationRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCabinConfiguration>(request);
        var id = await _sender.Send(command, ct);
        var cabinConfiguration = await _uow.CabinConfigurations.GetByIdAsync(id, ct);
        if (cabinConfiguration is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CabinConfigurationDto>(cabinConfiguration);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCabinConfigurationRequest request, CancellationToken ct)
    {
        var existing = await _uow.CabinConfigurations.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCabinConfiguration>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var cabinConfiguration = await _uow.CabinConfigurations.GetByIdAsync(id, ct);
        if (cabinConfiguration is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteCabinConfiguration(id), ct);

        return NoContent();
    }
}
