using Api.Dtos.Addresses;
using Application.Abstractions;
using Application.UseCase.Addresses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AddressesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AddressesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AddressDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AddressDto>>> GetAll(CancellationToken ct)
    {
        var addresses = await _uow.Addresses.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AddressDto>>(addresses);
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

        var addresses = await _uow.Addresses.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Addresses.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AddressDto>>(addresses);

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
        var total = await _uow.Addresses.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressDto>> GetById(int id, CancellationToken ct)
    {
        var address = await _uow.Addresses.GetByIdAsync(id, ct);
        if (address is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AddressDto>(address);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAddressRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAddress>(request);
        var id = await _sender.Send(command, ct);
        var address = await _uow.Addresses.GetByIdAsync(id, ct);
        if (address is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AddressDto>(address);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressRequest request, CancellationToken ct)
    {
        var existing = await _uow.Addresses.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAddress>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var address = await _uow.Addresses.GetByIdAsync(id, ct);
        if (address is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAddress(id), ct);

        return NoContent();
    }
}
