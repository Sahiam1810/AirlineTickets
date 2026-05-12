namespace Domain.ValueObjects.Fares;

public sealed record DateRange
{
    public DateOnly? ValidFrom { get; }
    public DateOnly? ValidTo { get; }

    private DateRange(DateOnly? validFrom, DateOnly? validTo)
    {
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public static DateRange Create(DateOnly? validFrom, DateOnly? validTo)
    {
        Validate(validFrom, validTo);
        return new DateRange(validFrom, validTo);
    }

    public static void Validate(DateOnly? validFrom, DateOnly? validTo)
    {
        if (validFrom.HasValue && validTo.HasValue && validFrom.Value > validTo.Value)
            throw new ArgumentException("La fecha de vigencia inicial no puede ser mayor que la fecha de vigencia final");
    }
}
