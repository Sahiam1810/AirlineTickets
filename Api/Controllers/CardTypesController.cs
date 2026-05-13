using Api.Dtos.CardTypes;
using Application.Abstractions;
using Application.UseCase.CardTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CardTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CardTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CardTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CardTypeDto>>> GetAll(CancellationToken ct)
    {
        var cardTypes = await _uow.CardTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CardTypeDto>>(cardTypes);
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

        var cardTypes = await _uow.CardTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.CardTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CardTypeDto>>(cardTypes);

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
        var total = await _uow.CardTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CardTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CardTypeDto>> GetById(int id, CancellationToken ct)
    {
        var cardType = await _uow.CardTypes.GetByIdAsync(id, ct);
        if (cardType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CardTypeDto>(cardType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CardTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCardTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCardType>(request);
        var id = await _sender.Send(command, ct);
        var cardType = await _uow.CardTypes.GetByIdAsync(id, ct);
        if (cardType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CardTypeDto>(cardType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCardTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.CardTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCardType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeleteCardType(id);
        try
        {
            await _sender.Send(command, ct);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
