using Api.Dtos.PersonPhones;
using Application.Abstractions;
using Application.UseCase.PersonPhones;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PersonPhonesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PersonPhonesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PersonPhoneDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PersonPhoneDto>>> GetAll(CancellationToken ct)
    {
        var personPhones = await _uow.PersonPhones.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PersonPhoneDto>>(personPhones);
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

        var personPhones = await _uow.PersonPhones.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.PersonPhones.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PersonPhoneDto>>(personPhones);

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
        var total = await _uow.PersonPhones.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonPhoneDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonPhoneDto>> GetById(int id, CancellationToken ct)
    {
        var personPhone = await _uow.PersonPhones.GetByIdAsync(id, ct);
        if (personPhone is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PersonPhoneDto>(personPhone);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonPhoneDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePersonPhoneRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePersonPhone>(request);
        var id = await _sender.Send(command, ct);
        var personPhone = await _uow.PersonPhones.GetByIdAsync(id, ct);
        if (personPhone is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PersonPhoneDto>(personPhone);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonPhoneRequest request, CancellationToken ct)
    {
        var existing = await _uow.PersonPhones.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePersonPhone>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var personPhone = await _uow.PersonPhones.GetByIdAsync(id, ct);
        if (personPhone is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeletePersonPhone(id), ct);

        return NoContent();
    }
}
