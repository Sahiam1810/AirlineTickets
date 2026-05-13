using Api.Dtos.PaymentMethodTypes;
using Application.Abstractions;
using Application.UseCase.PaymentMethodTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PaymentMethodTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PaymentMethodTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentMethodTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PaymentMethodTypeDto>>> GetAll(CancellationToken ct)
    {
        var paymentMethodTypes = await _uow.PaymentMethodTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PaymentMethodTypeDto>>(paymentMethodTypes);
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

        var paymentMethodTypes = await _uow.PaymentMethodTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.PaymentMethodTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PaymentMethodTypeDto>>(paymentMethodTypes);

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
        var total = await _uow.PaymentMethodTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentMethodTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentMethodTypeDto>> GetById(int id, CancellationToken ct)
    {
        var paymentMethodType = await _uow.PaymentMethodTypes.GetByIdAsync(id, ct);
        if (paymentMethodType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentMethodTypeDto>(paymentMethodType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentMethodTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePaymentMethodType>(request);
        var id = await _sender.Send(command, ct);
        var paymentMethodType = await _uow.PaymentMethodTypes.GetByIdAsync(id, ct);
        if (paymentMethodType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentMethodTypeDto>(paymentMethodType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentMethodTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.PaymentMethodTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePaymentMethodType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeletePaymentMethodType(id);
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
